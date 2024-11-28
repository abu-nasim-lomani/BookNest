using BookNest.Models;
using BookNest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var userId = _userRepository.GetAllUsers().FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            var bookIssues = _bookIssueRepository.GetAllBookIssues().Where(bi => bi.UserId == userId);
            return View(bookIssues);
        }

        [HttpPost]
        public IActionResult ReturnBook(int bookIssueId)
        {
            var bookIssue = _bookIssueRepository.GetBookIssueById(bookIssueId);
            if (bookIssue == null || bookIssue.UserId != _userRepository.GetAllUsers().FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id)
            {
                return BadRequest("Book Issue does not exist or you are not authorized to return this book.");
            }

            bookIssue.ReturnDate = DateTime.Now;
            bookIssue.IsReturned = true;
            _bookIssueRepository.UpdateBookIssue(bookIssue);

            var book = _bookRepository.GetBookById(bookIssue.BookId);
            book.Quantity += 1; // পরিমাণ পুনরুদ্ধার করা
            _bookRepository.UpdateBook(book);

            return RedirectToAction("Index");
        }
    }
}
