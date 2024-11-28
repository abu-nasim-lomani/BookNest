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

        public bool IsAvailable { get; set; } = true;

        public ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
    }
}
