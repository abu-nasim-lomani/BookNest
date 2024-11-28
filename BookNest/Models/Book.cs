using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookNest.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        public bool IsAvailable
        {
            get { return Quantity > 0; }
        }

        public int Quantity { get; set; } // স্টকে থাকা বইয়ের পরিমাণ

        public ICollection<BookIssue> BookIssues { get; set; }

        public string? PendingUserId { get; set; } // পেন্ডিং ব্যবহারকারীর আইডি, nullable
    }
}
