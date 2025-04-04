using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using OnlineLibrary.ViewModels;
using OnlineLibrary.ViewModels.Book;
using System.Security.Claims;

namespace OnlineLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Book/Catalog
        public async Task<IActionResult> Catalog(string searchString, string genre, int pageIndex = 1)
        {
            const int pageSize = 12;

            var books = from b in _context.Books
                        select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                books = books.Where(b => b.Genre == genre);
            }

            var genres = await _context.Books
                .Select(b => b.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            var count = await books.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            pageIndex = Math.Max(1, Math.Min(pageIndex, totalPages));

            var bookList = await books
                .OrderByDescending(b => b.RegistrationDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
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
                    RegistrationDate = b.RegistrationDate
                })
                .ToListAsync();

            var viewModel = new BookCatalogViewModel
            {
                Books = bookList,
                SearchString = searchString,
                Genre = genre,
                Genres = genres,
                PageIndex = pageIndex,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                Description = book.Description,
                PublishedYear = book.PublishedYear,
                Available = book.Available,
                RegistrationDate = book.RegistrationDate
            };

            return View(viewModel);
        }

        // POST: Book/Borrow/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            if (!book.Available)
            {
                TempData["ErrorMessage"] = "This book is not available for borrowing.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if user already has 5 books borrowed
            var activeBorrowings = await _context.BorrowedBooks
                .Where(bb => bb.UserId == userId && bb.ReturnDate == null)
                .CountAsync();

            if (activeBorrowings >= 5)
            {
                TempData["ErrorMessage"] = "You cannot borrow more than 5 books at a time.";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Check if user already has this book borrowed
            var existingBorrowing = await _context.BorrowedBooks
                .FirstOrDefaultAsync(bb => bb.BookId == id && bb.UserId == userId && bb.ReturnDate == null);

            if (existingBorrowing != null)
            {
                TempData["ErrorMessage"] = "You have already borrowed this book.";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Create new borrowing record
            var borrowedBook = new BorrowedBook
            {
                BookId = id,
                UserId = userId,
                BorrowDate = DateTime.UtcNow
            };

            // Update book availability
            book.Available = false;

            _context.BorrowedBooks.Add(borrowedBook);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book borrowed successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Book/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = viewModel.Title,
                    Author = viewModel.Author,
                    AuthorId = viewModel.AuthorId,
                    Genre = viewModel.Genre,
                    Description = viewModel.Description,
                    PublishedYear = viewModel.PublishedYear,
                    Available = true,
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Add(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Book added successfully.";
                return RedirectToAction(nameof(Catalog));
            }
            return View(viewModel);
        }

        // GET: Book/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            // Check if user is admin or the author of the book
            var user = await _userManager.GetUserAsync(User);
            if (user.Role != "Admin" && book.AuthorId != user.Id)
            {
                return Forbid();
            }

            var viewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                Description = book.Description,
                PublishedYear = book.PublishedYear,
                Available = book.Available,
                RegistrationDate = book.RegistrationDate
            };

            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BookViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _context.Books.FindAsync(id);

                    if (book == null)
                    {
                        return NotFound();
                    }

                    // Check if user is admin or the author of the book
                    var user = await _userManager.GetUserAsync(User);
                    if (user.Role != "Admin" && book.AuthorId != user.Id)
                    {
                        return Forbid();
                    }

                    book.Title = viewModel.Title;
                    book.Author = viewModel.Author;
                    book.Genre = viewModel.Genre;
                    book.Description = viewModel.Description;
                    book.PublishedYear = viewModel.PublishedYear;

                    // Only admin can change availability
                    if (user.Role == "Admin")
                    {
                        book.Available = viewModel.Available;
                    }

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = viewModel.Id });
            }
            return View(viewModel);
        }

        // GET: Book/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                AuthorId = book.AuthorId,
                Genre = book.Genre,
                Description = book.Description,
                PublishedYear = book.PublishedYear,
                Available = book.Available,
                RegistrationDate = book.RegistrationDate
            };

            return View(viewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            // Check if book is currently borrowed
            var activeBorrowings = await _context.BorrowedBooks
                .AnyAsync(bb => bb.BookId == id && bb.ReturnDate == null);

            if (activeBorrowings)
            {
                TempData["ErrorMessage"] = "Cannot delete a book that is currently borrowed.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Catalog));
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}