using Microsoft.AspNetCore.Mvc;
using BookNest.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BookNest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var users = _userRepository.GetAllUsers();
            return View(users);
        }
    }
}
