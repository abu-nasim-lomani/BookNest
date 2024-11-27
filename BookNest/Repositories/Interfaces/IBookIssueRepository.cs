using System.Collections.Generic;
using BookNest.Models;

namespace BookNest.Repositories.Interfaces
{
    public interface IBookIssueRepository
    {
        BookIssue GetBookIssueById(int issueId);
        IEnumerable<BookIssue> GetAllBookIssues();
        void AddBookIssue(BookIssue bookIssue);
        void UpdateBookIssue(BookIssue bookIssue);
        void DeleteBookIssue(int issueId);
    }
}
