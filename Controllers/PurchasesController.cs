using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreApp.Data;
using BookstoreApp.Models;

using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace BookstoreApp.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly BookstoreContext _context;

        public PurchasesController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: Purchases/Create
        public async Task<IActionResult> Create(int? id)
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

            ViewBag.Book = book;
            return View();
        }

        // POST: Purchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId, string buyerName, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null || book.Stock < quantity)
            {
                return NotFound();
            }

            var purchase = new Purchase
            {
                BookId = bookId,
                BuyerName = buyerName,
                PurchaseDate = DateTime.Now,
                Quantity = quantity,
                TotalPrice = book.Price * quantity,
                IsPaid = false  // Initially, payment is not completed
            };

            book.Stock -= quantity;  // Update stock

            _context.Add(purchase);
            await _context.SaveChangesAsync();

            return RedirectToAction("Payment", new { id = purchase.Id });
        }

        // GET: Purchases/Payment/5
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.purchases.Include(p => p.Book).FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            var domain = "https://localhost:7122"; // Update with your domain

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
            {
                "card",
            },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(purchase.TotalPrice * 100), // Stripe expects amounts in cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = purchase.Book.Title,
                    },
                },
                Quantity = purchase.Quantity,
            },
        },
                Mode = "payment",
                SuccessUrl = domain + Url.Action("CompletePayment", new { id = purchase.Id }),
                CancelUrl = domain + Url.Action("Payment", new { id = purchase.Id }),
            };

            var service = new SessionService();
            Session session = service.Create(options);

            ViewBag.StripeSessionId = session.Id;
            ViewBag.StripePublishableKey = "pk_test_51PuWk2Rqdkw08sDX25IxNyghnLgGbdHwZgWEn1iC061ZxVRSUzIDqtXcBKzCf67xT4mbm8Tse9bKPEMdTQnugfi100IxjC0VjE"; // Replace with actual publishable key

            return View(purchase);
        }


        // GET: Purchases/CompletePayment
        public async Task<IActionResult> CompletePayment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            // Update the payment status
            purchase.IsPaid = true;
            _context.Update(purchase);
            await _context.SaveChangesAsync();

            return RedirectToAction("Receipt", new { id = purchase.Id });
        }

        // GET: Purchases/Receipt
        public async Task<IActionResult> Receipt(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.purchases.Include(p => p.Book).FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }
        // GET: Purchases/History
        // GET: Purchases/History
        public async Task<IActionResult> History(string sortOrder, string searchString)
        {
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";

            var purchases = await _context.purchases.Include(p => p.Book).AsNoTracking().ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower(); // Convert search string to lowercase

                purchases = purchases.Where(p =>
                    p.BuyerName.ToLower().Contains(searchString) ||
                    p.Book.Title.ToLower().Contains(searchString)
                ).ToList();
            }

            switch (sortOrder)
            {
                case "date_desc":
                    purchases = purchases.OrderByDescending(p => p.PurchaseDate).ToList();
                    break;
                case "Price":
                    purchases = purchases.OrderBy(p => (double)p.TotalPrice).ToList();
                    break;
                case "price_desc":
                    purchases = purchases.OrderByDescending(p => (double)p.TotalPrice).ToList();
                    break;
                default:
                    purchases = purchases.OrderBy(p => p.PurchaseDate).ToList();
                    break;
            }

            return View(purchases);
        }

    }
}

