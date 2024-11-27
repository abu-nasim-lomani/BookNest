using Microsoft.AspNetCore.Mvc;
using BookNest.Models;
using BookNest.Services;

namespace BookNest.Controllers
{
    public class BookIssueController : Controller
    {
        private readonly BookIssueService _bookIssueService;

        public BookIssueController(BookIssueService bookIssueService)
        {
            _bookIssueService = bookIssueService;
        }

        public IActionResult Index()
        {
            var bookIssues = _bookIssueService.GetAllBookIssues();
            return View(bookIssues);
        }

        public IActionResult Details(int id)
        {
            var bookIssue = _bookIssueService.GetBookIssueById(id);
            if (bookIssue == null)
            {
                return NotFound();
            }
            return View(bookIssue);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookIssue bookIssue)
        {
            if (ModelState.IsValid)
            {
                _bookIssueService.AddBookIssue(bookIssue);
                return RedirectToAction(nameof(Index));
            }
            return View(bookIssue);
        }

        public IActionResult Edit(int id)
        {
            var bookIssue = _bookIssueService.GetBookIssueById(id);
            if (bookIssue == null)
            {
                return NotFound();
            }
            return View(bookIssue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BookIssue bookIssue)
        {
            if (id != bookIssue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bookIssueService.UpdateBookIssue(bookIssue);
                return RedirectToAction(nameof(Index));
            }
            return View(bookIssue);
        }

        public IActionResult Delete(int id)
        {
            var bookIssue = _bookIssueService.GetBookIssueById(id);
            if (bookIssue == null)
            {
                return NotFound();
            }
            return View(bookIssue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookIssueService.DeleteBookIssue(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
