using BookNest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using BookNest.Repositories.Interfaces;
using BookNest.ViewModels;
using System.Linq;
using System;

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
                try
                {
                    _bookRepository.AddBook(book);
                    TempData["SuccessMessage"] = "Book added successfully!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorMessage"] = "Failed to add book. Errors: " + string.Join(", ", errors);
            }
            return RedirectToAction("AdminDashboard");
        }


        public IActionResult BookList(string searchTerm)
        {
            var books = string.IsNullOrEmpty(searchTerm) ? _bookRepository.GetAllBooks() : _bookRepository.SearchBooks(searchTerm);
            return View(books);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
