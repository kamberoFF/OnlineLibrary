using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.ViewModels.Book;
using OnlineLibrary.ViewModels.Dashboard;
using System.Security.Claims;

namespace OnlineLibrary.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            // Get borrowed books
            var borrowedBooks = await _context.BorrowedBooks
                .Where(bb => bb.UserId == userId)
                .OrderByDescending(bb => bb.BorrowDate)
                .Join(_context.Books,
                    bb => bb.BookId,
                    b => b.Id,
                    (bb, b) => new BorrowedBookViewModel
                    {
                        Id = bb.Id,
                        BookId = bb.BookId,
                        BookTitle = b.Title,
                        BookAuthor = b.Author,
                        BorrowDate = bb.BorrowDate,
                        ReturnDate = bb.ReturnDate
                    })
                .ToListAsync();

            // Get reading history
            var readingHistory = await _context.ReadingHistory
                .Where(rh => rh.UserId == userId)
                .OrderByDescending(rh => rh.ReadDate)
                .Join(_context.Books,
                    rh => rh.BookId,
                    b => b.Id,
                    (rh, b) => new ReadingHistoryViewModel
                    {
                        Id = rh.Id,
                        BookId = rh.BookId,
                        BookTitle = b.Title,
                        BookAuthor = b.Author,
                        ReadDate = rh.ReadDate,
                        Rating = rh.Rating,
                        Review = rh.Review
                    })
                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                BorrowedBooks = borrowedBooks,
                ReadingHistory = readingHistory,
                User = user
            };

            return View(viewModel);
        }

        // POST: Dashboard/ReturnBook/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int id, bool addReview = false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var borrowedBook = await _context.BorrowedBooks
                .FirstOrDefaultAsync(bb => bb.Id == id && bb.UserId == userId);

            if (borrowedBook == null)
            {
                return NotFound();
            }

            if (borrowedBook.ReturnDate != null)
            {
                TempData["ErrorMessage"] = "This book has already been returned.";
                return RedirectToAction(nameof(Index));
            }

            // Update borrowed book record
            borrowedBook.ReturnDate = DateTime.UtcNow;

            // Update book availability
            var book = await _context.Books.FindAsync(borrowedBook.BookId);
            if (book != null)
            {
                book.AvailableToBorrow = true;
            }

            // Add to reading history
            var readingHistory = new ReadingHistory
            {
                BookId = borrowedBook.BookId,
                UserId = userId,
                ReadDate = DateTime.UtcNow,
                Rating = 0, // Default rating
                Review = "" // Empty review
            };

            _context.ReadingHistory.Add(readingHistory);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book returned successfully.";

            // If user wants to add a review immediately
            if (addReview)
            {
                return RedirectToAction(nameof(AddReview), new { id = readingHistory.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Dashboard/AddReview/5
        public async Task<IActionResult> AddReview(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingHistory = await _context.ReadingHistory
                .FirstOrDefaultAsync(rh => rh.Id == id && rh.UserId == userId);

            if (readingHistory == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(readingHistory.BookId);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new ReadingHistoryViewModel
            {
                Id = readingHistory.Id,
                BookId = readingHistory.BookId,
                BookTitle = book.Title,
                BookAuthor = book.Author,
                ReadDate = readingHistory.ReadDate,
                Rating = readingHistory.Rating,
                Review = readingHistory.Review
            };

            return View(viewModel);
        }

        // POST: Dashboard/AddReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int id, ReadingHistoryViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var readingHistory = await _context.ReadingHistory
                .FirstOrDefaultAsync(rh => rh.Id == id && rh.UserId == userId);

            if (readingHistory == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                readingHistory.Rating = viewModel.Rating;
                readingHistory.Review = viewModel.Review;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Review added successfully.";
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            var book = await _context.Books.FindAsync(readingHistory.BookId);
            viewModel.BookTitle = book?.Title;
            viewModel.BookAuthor = book?.Author;
            return View(viewModel);
        }
    }
}