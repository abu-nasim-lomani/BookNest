using System.Collections.Generic;
using System.Linq;
using BookNest.Data;
using BookNest.Models;
using BookNest.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.Include(b => b.BookIssues).FirstOrDefault(b => b.Id == bookId);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.Include(b => b.BookIssues).ToList();
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public void DeleteBook(int bookId)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
