using BookNest.Models;
using BookNest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookNest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookIssueRequestRepository _bookIssueRequestRepository;
        private readonly IBookIssueRepository _bookIssueRepository;

        public AdminController(IUserRepository userRepository, IBookRepository bookRepository, IBookIssueRequestRepository bookIssueRequestRepository, IBookIssueRepository bookIssueRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _bookIssueRequestRepository = bookIssueRequestRepository;
            _bookIssueRepository = bookIssueRepository;
        }

        public IActionResult ReviewRequests()
        {
            var requests = _bookIssueRequestRepository.GetAllRequests();
            return View(requests);
        }

        [HttpPost]
        public IActionResult ApproveRequest(int requestId)
        {
            var request = _bookIssueRequestRepository.GetRequestById(requestId);
            if (request == null)
            {
                return BadRequest("Request does not exist.");
            }

            request.IsApproved = true;
            request.IsPending = false;
            _bookIssueRequestRepository.UpdateRequest(request);

            var bookIssue = new BookIssue
            {
                BookId = request.BookId,
                UserId = request.UserId,
                IssueDate = DateTime.Now,
                DueDate = request.ReturnDate,
                IsReturned = false
            };

            _bookIssueRepository.AddBookIssue(bookIssue);
            _bookIssueRequestRepository.DeleteRequest(requestId);

            return RedirectToAction("ReviewRequests");
        }

        [HttpPost]
        public IActionResult DeclineRequest(int requestId)
        {
            var request = _bookIssueRequestRepository.GetRequestById(requestId);
            if (request == null)
            {
                return BadRequest("Request does not exist.");
            }

            _bookIssueRequestRepository.DeleteRequest(requestId);

            var book = _bookRepository.GetBookById(request.BookId);
            book.Quantity += 1;
            _bookRepository.UpdateBook(book);

            return RedirectToAction("ReviewRequests");
        }

        public IActionResult UserIssues()
        {
            var userIssues = _userRepository.GetAllUsers()
                .Select(u => new UserIssueViewModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    BookIssues = _bookIssueRepository.GetAllBookIssues()
                        .Where(bi => bi.UserId == u.Id && !bi.IsReturned)
                        .Select(bi => new BookIssueViewModel
                        {
                            BookTitle = bi.Book.Title,
                            IssueDate = bi.IssueDate,
                            DueDate = bi.DueDate,
                            IsExpired = bi.DueDate < DateTime.Now && !bi.IsReturned
                        }).ToList(),
                    IsRestricted = u.IsRestricted
                }).ToList();

            return View(userIssues);
        }

        [HttpPost]
        public IActionResult RestrictUser(string userId)
        {
            var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            user.IsRestricted = true;
            _userRepository.UpdateUser(user);

            return RedirectToAction("UserIssues");
        }

        [HttpPost]
        public IActionResult UnrestrictUser(string userId)
        {
            var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            user.IsRestricted = false;
            _userRepository.UpdateUser(user);

            return RedirectToAction("UserIssues");
        }
    }
}
