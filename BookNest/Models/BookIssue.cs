using System;

namespace BookNest.Models
{
    public class BookIssue
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;

        public Book Book { get; set; }
        public User User { get; set; }
    }
}
