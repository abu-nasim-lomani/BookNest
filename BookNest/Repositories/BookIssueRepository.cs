using System.Collections.Generic;
using System.Linq;
using BookNest.Data;
using BookNest.Models;
using BookNest.Repositories.Interfaces;
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

        public BookIssue GetBookIssueById(int issueId)
        {
            return _context.BookIssues.Include(bi => bi.Book).Include(bi => bi.User).FirstOrDefault(bi => bi.Id == issueId);
        }

        public IEnumerable<BookIssue> GetAllBookIssues()
        {
            return _context.BookIssues.Include(bi => bi.Book).Include(bi => bi.User).ToList();
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

        public void DeleteBookIssue(int issueId)
        {
            var bookIssue = _context.BookIssues.Find(issueId);
            if (bookIssue != null)
            {
                _context.BookIssues.Remove(bookIssue);
                _context.SaveChanges();
            }
        }
    }
}
