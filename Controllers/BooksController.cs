using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreApp.Data;
using BookstoreApp.Models;
namespace BookstoreApp
{
    public class BooksController : Controller
    {
        private readonly BookstoreContext _context;

        public BooksController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: Books pagination
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var books = await _context.Books
                                      .OrderBy(b => b.Title)
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            int totalBooks = await _context.Books.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(books);
        }


        // GET and POST: Search 
        public async Task<IActionResult> Search(string searchterm, int? page)
        {
            var books = from b in _context.Books
                        select b;

            if (!string.IsNullOrEmpty(searchterm))
            {
                searchterm = searchterm.ToLower();
                books = books.Where(x => x.Title.ToLower().Contains(searchterm) ||
                                          x.Author.ToLower().Contains(searchterm) ||
                                          x.Genre.ToLower().Contains(searchterm));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagedBooks = await books.OrderBy(b => b.Title)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            int totalBooks = await books.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double) totalBooks / pageSize);
            ViewBag.CurrentPage = pageNumber;

            if (!pagedBooks.Any())
            {
                ViewBag.Message = "No books found.";
            }

            return View("Index", pagedBooks);
        }
        // GET: Books/Details
        public async Task<IActionResult> Details(int? id)
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

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Genre,Price,Stock")] Book book)
        {
            if (ModelState.IsValid)
            {
                // Server-side duplicate check
                var isDuplicate = await _context.Books.AnyAsync(b => b.Title.ToLower() == book.Title.ToLower() && b.Author.ToLower() == book.Author.ToLower());
                if (isDuplicate)
                {
                    ModelState.AddModelError("", "This book already exists.");
                    return View(book);
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit
        public async Task<IActionResult> Edit(int? id)
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
            return View(book);
        }

        // POST: Books/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,Price,Stock")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete
        public async Task<IActionResult> Delete(int? id)
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

            return View(book);
        }

        // POST: Books/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
         
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
