using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.ViewModels;
using OnlineLibrary.ViewModels.Book;
using OnlineLibrary.ViewModels.Dashboard;

namespace OnlineLibrary.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var totalBooks = await _context.Books.CountAsync();
            var totalUsers = await _userManager.Users.CountAsync();
            var totalBorrowings = await _context.BorrowedBooks.CountAsync();
            var totalReviews = await _context.ReadingHistory.Where(rh => rh.Rating != 0).CountAsync();

            ViewBag.TotalBooks = totalBooks;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalBorrowings = totalBorrowings;
            ViewBag.TotalReviews = totalReviews;

            return View();
        }

        // GET: Admin/Books
        public async Task<IActionResult> Books(string searchString, string sortOrder)
        {
            ViewData["TitleSortParam"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParam"] = sortOrder == "author" ? "author_desc" : "author";
            ViewData["YearSortParam"] = sortOrder == "year" ? "year_desc" : "year";
            ViewData["CurrentFilter"] = searchString;

            var books = from b in _context.Books
                        select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "author":
                    books = books.OrderBy(b => b.Author);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.Author);
                    break;
                case "year":
                    books = books.OrderBy(b => b.PublishedYear);
                    break;
                case "year_desc":
                    books = books.OrderByDescending(b => b.PublishedYear);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            var bookList = await books
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    AuthorId = b.AuthorId,
                    Genre = b.Genre,
                    Description = b.Description,
                    PublishedYear = b.PublishedYear,
                    Available = b.Available,
                    AvailableToBorrow = b.AvailableToBorrow,
                    RegistrationDate = b.RegistrationDate
                })
                .ToListAsync();

            return View(bookList);
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();

            return View(users);
        }

        // GET: Admin/UserDetails/5
        public async Task<IActionResult> UserDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Get borrowed books
            var borrowedBooks = await _context.BorrowedBooks
                .Where(bb => bb.UserId == id)
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
                .Where(rh => rh.UserId == id)
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

        // GET: Admin/EditUser/5
        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new AdminEditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            return View(viewModel);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, AdminEditUserViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.PhoneNumber = viewModel.PhoneNumber;
                user.Role = viewModel.Role;

                // Update email if changed
                if (user.Email != viewModel.Email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, viewModel.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        foreach (var error in setEmailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(viewModel);
                    }
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User updated successfully.";
                    return RedirectToAction(nameof(Users));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(viewModel);
        }

        // GET: Admin/Borrowings
        public async Task<IActionResult> Borrowings(bool activeOnly = true)
        {
            var borrowings = from bb in _context.BorrowedBooks
                             join b in _context.Books on bb.BookId equals b.Id
                             join u in _userManager.Users on bb.UserId equals u.Id
                             select new AdminBorrowingViewModel
                             {
                                 Id = bb.Id,
                                 BookId = bb.BookId,
                                 BookTitle = b.Title,
                                 UserId = bb.UserId,
                                 UserName = u.FirstName + " " + u.LastName,
                                 BorrowDate = bb.BorrowDate,
                                 ReturnDate = bb.ReturnDate,
                                 DueDate = bb.BorrowDate.AddDays(14)
                             };

            if (activeOnly)
            {
                borrowings = borrowings.Where(bb => bb.ReturnDate == null);
            }

            var borrowingsList = await borrowings
                .OrderByDescending(bb => bb.BorrowDate)
                .ToListAsync();

            ViewBag.ActiveOnly = activeOnly;

            return View(borrowingsList);
        }

        // POST: Admin/ReturnBook/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var borrowedBook = await _context.BorrowedBooks.FindAsync(id);
            if (borrowedBook == null)
            {
                return NotFound();
            }

            if (borrowedBook.ReturnDate != null)
            {
                TempData["ErrorMessage"] = "This book has already been returned.";
                return RedirectToAction(nameof(Borrowings));
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
                UserId = borrowedBook.UserId,
                ReadDate = DateTime.UtcNow,
                Rating = 0, // Default rating
                Review = "" // Empty review
            };

            _context.ReadingHistory.Add(readingHistory);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book returned successfully.";
            return RedirectToAction(nameof(Borrowings));
        }

        // GET: Admin/Reviews
        public async Task<IActionResult> Reviews()
        {
            var reviews = from rh in _context.ReadingHistory
                          join b in _context.Books on rh.BookId equals b.Id
                          join u in _userManager.Users on rh.UserId equals u.Id
                          where rh.Rating > 0 || !string.IsNullOrEmpty(rh.Review)
                          select new AdminReviewViewModel
                          {
                              Id = rh.Id,
                              BookId = rh.BookId,
                              BookTitle = b.Title,
                              UserId = rh.UserId,
                              UserName = u.FirstName + " " + u.LastName,
                              ReadDate = rh.ReadDate,
                              Rating = rh.Rating,
                              Review = rh.Review
                          };

            var reviewsList = await reviews
                .OrderByDescending(r => r.ReadDate)
                .ToListAsync();

            return View(reviewsList);
        }

        // GET: Admin/ReviewDetails/5
        public async Task<IActionResult> ReviewDetails(int id)
        {
            var review = await _context.ReadingHistory
                .FirstOrDefaultAsync(rh => rh.Id == id);

            // Address this with relations
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == review.UserId);

            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == review.BookId);

            if (review == null)
            {
                return NotFound();
            }

            var viewModel = new AdminReviewViewModel
            {
                Id = review.Id,
                BookId = review.BookId,
                BookTitle = book.Title,
                UserId = review.UserId,
                UserName = $"{user.FirstName} {user.LastName}",
                ReadDate = review.ReadDate,
                Rating = review.Rating,
                Review = review.Review
            };

            return View(viewModel);
        }

        // POST: Admin/DeleteReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.ReadingHistory.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            _context.ReadingHistory.Remove(review);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Review removed successfully.";
            return RedirectToAction(nameof(Reviews));
        }
    }
}