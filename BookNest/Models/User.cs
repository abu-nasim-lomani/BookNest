using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BookNest.Models
{
    public class User : IdentityUser
    {
        public bool IsRestricted { get; set; } = false;
        public ICollection<BookIssue> BookIssues { get; set; }
    }
}
