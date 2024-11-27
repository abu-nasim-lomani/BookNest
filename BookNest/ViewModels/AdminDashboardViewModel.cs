using BookNest.Models;
using System.Collections.Generic;

namespace BookNest.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
