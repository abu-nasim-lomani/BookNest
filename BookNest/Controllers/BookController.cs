using Microsoft.AspNetCore.Mvc;
using BookNest.Repositories.Interfaces;
using BookNest.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookNest.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetAllBooks();
            return View(books);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddBook(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.AddBook(book);
                return RedirectToAction("Index");
            }
            return View(book);
        }
    }
}
