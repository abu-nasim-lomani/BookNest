﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BookNest.Models
{
    public class BookIssue
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
        public DateTime IssueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned
        {
            get { return ReturnDate.HasValue; }
        }

        public DateTime DueDate { get; set; } // নতুন প্রপার্টি
    }
}
