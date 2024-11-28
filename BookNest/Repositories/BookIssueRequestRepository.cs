using BookNest.Data;
using BookNest.Models;
using BookNest.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Repositories
{
    public class BookIssueRequestRepository : IBookIssueRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public BookIssueRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BookIssueRequest> GetAllRequests()
        {
            return _context.BookIssueRequests.Include(r => r.Book).Include(r => r.User).ToList();
        }

        public BookIssueRequest GetRequestById(int id)
        {
            return _context.BookIssueRequests.Include(r => r.Book).Include(r => r.User).FirstOrDefault(r => r.Id == id);
        }

        public void AddRequest(BookIssueRequest request)
        {
            _context.BookIssueRequests.Add(request);
            _context.SaveChanges();
        }

        public void UpdateRequest(BookIssueRequest request)
        {
            _context.BookIssueRequests.Update(request);
            _context.SaveChanges();
        }

        public void DeleteRequest(int id)
        {
            var request = _context.BookIssueRequests.Find(id);
            if (request != null)
            {
                _context.BookIssueRequests.Remove(request);
                _context.SaveChanges();
            }
        }
    }
}
