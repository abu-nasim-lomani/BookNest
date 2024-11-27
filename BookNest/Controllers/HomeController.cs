using BookNest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using BookNest.Repositories.Interfaces;
using BookNest.ViewModels;

namespace BookNest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IBookRepository bookRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            var users = _userRepository.GetAllUsers();
            var books = _bookRepository.GetAllBooks();
            var viewModel = new AdminDashboardViewModel
            {
                Users = users,
                Books = books
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.AddBook(book);
                return RedirectToAction("AdminDashboard");
            }
            return View("AdminDashboard", book);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
