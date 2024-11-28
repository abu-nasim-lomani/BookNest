using System;
using System.ComponentModel.DataAnnotations;

namespace BookNest.Models
{
    public class BookIssueRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        public bool IsApproved { get; set; }
        public bool IsPending { get; set; } = true; // পেন্ডিং স্ট্যাটাস
    }
}
