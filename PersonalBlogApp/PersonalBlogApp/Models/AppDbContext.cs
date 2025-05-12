using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace PersonalBlogApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>();

            modelBuilder.Entity<Blog>()
            .HasOne(b => b.User)          
            .WithMany(u => u.Blogs)       
            .HasForeignKey(b => b.User);

            modelBuilder.Entity<Comment>()
                .HasOne(b => b.Blog)
                .WithMany(u => u.Comments);
                
            // base.OnModelCreating(modelBuilder);
        }
    }
}
