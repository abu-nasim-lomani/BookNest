using BookNest.Models;
using System.Collections.Generic;

namespace BookNest.Repositories.Interfaces
{
    public interface IBookIssueRequestRepository
    {
        IEnumerable<BookIssueRequest> GetAllRequests();
        BookIssueRequest GetRequestById(int id);
        void AddRequest(BookIssueRequest request);
        void UpdateRequest(BookIssueRequest request);
        void DeleteRequest(int id);
    }
}
