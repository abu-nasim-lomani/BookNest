using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookNest.Models;

namespace BookNest.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookIssue> BookIssues { get; set; }

        public DbSet<BookIssueRequest> BookIssueRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookIssue>()
                .HasOne(bi => bi.Book)
                .WithMany(b => b.BookIssues)
                .HasForeignKey(bi => bi.BookId);

            builder.Entity<BookIssue>()
                .HasOne(bi => bi.User)
                .WithMany(u => u.BookIssues)
                .HasForeignKey(bi => bi.UserId);
        }
    }
}
