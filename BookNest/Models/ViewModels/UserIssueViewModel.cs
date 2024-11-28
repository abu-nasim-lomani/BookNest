using System.Collections.Generic;

namespace BookNest.Models
{
    public class UserIssueViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<BookIssueViewModel> BookIssues { get; set; }
        public bool IsRestricted { get; set; }
    }
}
