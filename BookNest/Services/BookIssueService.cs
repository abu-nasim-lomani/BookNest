using System.Collections.Generic;
using BookNest.Models;
using BookNest.Repositories.Interfaces;

namespace BookNest.Services
{
    public class BookIssueService
    {
        private readonly IBookIssueRepository _bookIssueRepository;

        public BookIssueService(IBookIssueRepository bookIssueRepository)
        {
            _bookIssueRepository = bookIssueRepository;
        }

        public BookIssue GetBookIssueById(int issueId)
        {
            return _bookIssueRepository.GetBookIssueById(issueId);
        }

        public IEnumerable<BookIssue> GetAllBookIssues()
        {
            return _bookIssueRepository.GetAllBookIssues();
        }

        public void AddBookIssue(BookIssue bookIssue)
        {
            _bookIssueRepository.AddBookIssue(bookIssue);
        }

        public void UpdateBookIssue(BookIssue bookIssue)
        {
            _bookIssueRepository.UpdateBookIssue(bookIssue);
        }

        public void DeleteBookIssue(int issueId)
        {
            _bookIssueRepository.DeleteBookIssue(issueId);
        }
    }
}
