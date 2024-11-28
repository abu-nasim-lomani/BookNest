using BookNest.Models;
using BookNest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookNest.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookIssueRequestRepository _bookIssueRequestRepository;
        private readonly IUserRepository _userRepository;

        public BookController(IBookRepository bookRepository, IBookIssueRequestRepository bookIssueRequestRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _bookIssueRequestRepository = bookIssueRequestRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetAllBooks();
            return View(books);
        }

        [HttpPost("Book/RequestIssue")]
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

            return RedirectToAction("Index");
        }
    }
}
