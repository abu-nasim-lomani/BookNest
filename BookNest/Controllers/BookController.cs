using BookNest.Models;
using BookNest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace BookNest.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookIssueRequestRepository _bookIssueRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public BookController(IBookRepository bookRepository, IBookIssueRequestRepository bookIssueRequestRepository, IUserRepository userRepository, UserManager<User> userManager)
        {
            _bookRepository = bookRepository;
            _bookIssueRequestRepository = bookIssueRequestRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetAllBooks();
            var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.UserName == User.Identity.Name);
            ViewBag.IsAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
            return View(books);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                var existingBook = _bookRepository.GetBookById(book.Id);
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Quantity = book.Quantity;
                _bookRepository.UpdateBook(existingBook);
                TempData["SuccessMessage"] = "Book updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to update book.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteBook(int bookId)
        {
            _bookRepository.DeleteBook(bookId);
            TempData["SuccessMessage"] = "Book deleted successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RequestIssue(int bookId, DateTime returnDate, string returnTime)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null || !book.IsAvailable)
            {
                return BadRequest("Book not available or does not exist.");
            }

            var userId = _userRepository.GetAllUsers().FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            if (userId == null)
            {
                return BadRequest("User does not exist.");
            }

            var returnDateTime = returnDate.Add(TimeSpan.Parse(returnTime));

            var request = new BookIssueRequest
            {
                BookId = bookId,
                UserId = userId,
                RequestDate = DateTime.Now,
                ReturnDate = returnDateTime,
                IsApproved = false
            };

            _bookIssueRequestRepository.AddRequest(request);

            // বইটির পেন্ডিং ইউজার আপডেট করা
            book.PendingUserId = userId;
            _bookRepository.UpdateBook(book);

            return RedirectToAction("Index");
        }
    }
}
