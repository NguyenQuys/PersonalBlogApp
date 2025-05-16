using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using PersonalBlogApp.Requests;

namespace PersonalBlogApp.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Comment>()
                .HasOne(b=>b.Blog)
                .WithMany(u => u.Comments)
                .HasForeignKey(b => b.BlogId);

            modelBuilder.Entity<Comment>()
                .HasOne(u=>u.User)
                .WithMany(c=>c.Comments)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict); ;
            
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<PersonalBlogApp.Requests.BlogRequest> BlogRequest { get; set; } = default!;
    }
}
