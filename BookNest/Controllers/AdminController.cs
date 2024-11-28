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

            var userIssues = _bookIssueRepository.GetAllBookIssues().Where(bi => bi.UserId == request.UserId && !bi.IsReturned);
            if (userIssues.Any())
            {
                return BadRequest("User has unreturned books.");
            }

            request.IsApproved = true;
            _bookIssueRequestRepository.UpdateRequest(request);

            var bookIssue = new BookIssue
            {
                BookId = request.BookId,
                UserId = request.UserId,
                IssueDate = DateTime.Now,
                DueDate = request.ReturnDate
            };

            _bookIssueRepository.AddBookIssue(bookIssue);

            var book = _bookRepository.GetBookById(request.BookId);
            book.Quantity -= 1; // পরিমাণ হ্রাস করা
            book.PendingUserId = null; // পেন্ডিং ইউজার সাফ করা
            _bookRepository.UpdateBook(book);

            _bookIssueRequestRepository.DeleteRequest(requestId); // অনুরোধ মুছে ফেলা

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

            return RedirectToAction("ReviewRequests");
        }

        [HttpPost]
        public IActionResult CheckUserIssues(string userId)
        {
            var userIssues = _bookIssueRepository.GetAllBookIssues().Where(bi => bi.UserId == userId && !bi.IsReturned);
            return PartialView("_UserIssuesPartial", userIssues);
        }
    }
}
