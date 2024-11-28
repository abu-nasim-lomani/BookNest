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
        private readonly IBookIssueRepository _bookIssueRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public BookController(IBookRepository bookRepository, IBookIssueRequestRepository bookIssueRequestRepository, IBookIssueRepository bookIssueRepository, IUserRepository userRepository, UserManager<User> userManager)
        {
            _bookRepository = bookRepository;
            _bookIssueRequestRepository = bookIssueRequestRepository;
            _bookIssueRepository = bookIssueRepository;
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
        public IActionResult RequestIssue(int bookId, DateTime returnDate)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null || book.Quantity <= 0)
            {
                return BadRequest("Book not available or does not exist.");
            }

            var userId = _userRepository.GetAllUsers().FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            if (userId == null)
            {
                return BadRequest("User does not exist.");
            }

            // চেক করা যে ব্যবহারকারীর কাছে কোনো ইস্যু করা বই আছে কিনা যা ফেরত করা হয়নি
            var userIssues = _bookIssueRepository.GetAllBookIssues().Where(bi => bi.UserId == userId && !bi.IsReturned);
            if (userIssues.Any())
            {
                return BadRequest("Please return your previous book before issuing a new one.");
            }

            var request = new BookIssueRequest
            {
                BookId = bookId,
                UserId = userId,
                RequestDate = DateTime.Now,
                ReturnDate = returnDate,
                IsApproved = false,
                IsPending = true
            };

            _bookIssueRequestRepository.AddRequest(request);

            return Ok();
        }
    }
}
