using BookNest.Data;
using BookNest.Models;
using BookNest.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Repositories
{
    public class BookIssueRepository : IBookIssueRepository
    {
        private readonly ApplicationDbContext _context;

        public BookIssueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BookIssue> GetAllBookIssues()
        {
            return _context.BookIssues
                .Include(bi => bi.Book)
                .Include(bi => bi.User)
                .ToList();
        }

        public BookIssue GetBookIssueById(int id)
        {
            return _context.BookIssues
                .Include(bi => bi.Book)
                .Include(bi => bi.User)
                .FirstOrDefault(bi => bi.Id == id);
        }

        public void AddBookIssue(BookIssue bookIssue)
        {
            _context.BookIssues.Add(bookIssue);
            _context.SaveChanges();
        }

        public void UpdateBookIssue(BookIssue bookIssue)
        {
            _context.BookIssues.Update(bookIssue);
            _context.SaveChanges();
        }

        public void DeleteBookIssue(int id)
        {
            var bookIssue = _context.BookIssues.Find(id);
            if (bookIssue != null)
            {
                _context.BookIssues.Remove(bookIssue);
                _context.SaveChanges();
            }
        }
    }
}
