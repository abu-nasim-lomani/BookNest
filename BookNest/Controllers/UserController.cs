using BookNest.Models;
using BookNest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookNest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookIssueRepository _bookIssueRepository;

        public UserController(IUserRepository userRepository, IBookIssueRepository bookIssueRepository)
        {
            _userRepository = userRepository;
            _bookIssueRepository = bookIssueRepository;
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetAllUsers();
            return View(users);
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
