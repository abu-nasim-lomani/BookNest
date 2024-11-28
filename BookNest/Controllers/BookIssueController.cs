using BookNest.Models;
using BookNest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookNest.Controllers
{
    [Authorize]
    public class BookIssueController : Controller
    {
        private readonly IBookIssueRepository _bookIssueRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public BookIssueController(IBookIssueRepository bookIssueRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _bookIssueRepository = bookIssueRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var bookIssues = _bookIssueRepository.GetAllBookIssues();
            return View(bookIssues);
        }

        [HttpPost]
        public IActionResult IssueBook(int bookId, string userId)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null || !book.IsAvailable)
            {
                return BadRequest("Book not available or does not exist.");
            }

            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            var bookIssue = new BookIssue
            {
                BookId = bookId,
                UserId = userId,
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14) // নির্দিষ্ট সময়ের মধ্যে (১৪ দিন)
            };

            _bookIssueRepository.AddBookIssue(bookIssue);
            book.IsAvailable = false;
            _bookRepository.UpdateBook(book);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ReturnBook(int bookIssueId)
        {
            var bookIssue = _bookIssueRepository.GetBookIssueById(bookIssueId);
            if (bookIssue == null)
            {
                return BadRequest("Book Issue does not exist.");
            }

            bookIssue.ReturnDate = DateTime.Now;
            _bookIssueRepository.UpdateBookIssue(bookIssue);

            var book = _bookRepository.GetBookById(bookIssue.BookId);
            book.IsAvailable = true;
            _bookRepository.UpdateBook(book);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CheckRestrictions()
        {
            var overdueIssues = _bookIssueRepository.GetAllBookIssues().Where(bi => bi.DueDate < DateTime.Now && !bi.IsReturned);
            foreach (var issue in overdueIssues)
            {
                var user = _userRepository.GetUserById(issue.UserId);
                if (user != null)
                {
                    user.IsRestricted = true;
                    _userRepository.UpdateUser(user);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
