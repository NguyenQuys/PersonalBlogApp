using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Blog - User
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Blog - comment
            modelBuilder.Entity<Comment>()
                .HasOne(c=>c.Blog)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - cmt
            modelBuilder.Entity<Comment>()
                .HasOne(c=>c.User)
                .WithMany(u=>u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment - Self (Parent - Replies)
            modelBuilder.Entity<Comment>().
                HasOne(r => r.Parent)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Like - blog
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Blog)
                .WithMany(b => b.Likes)
                .HasForeignKey(l=>l.BlogId)
                .OnDelete(DeleteBehavior.Restrict);

            // Like - user 
            modelBuilder.Entity<Like>()
                .HasOne(u=>u.Users)
                .WithMany(m=>m.Likes)
                .HasForeignKey(u=>u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.BlogId })
                .IsUnique();
        }
    }
}
