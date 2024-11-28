using System;

namespace BookNest.Models
{
    public class BookIssueViewModel
    {
        public string BookTitle { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsExpired { get; set; }
    }
}
