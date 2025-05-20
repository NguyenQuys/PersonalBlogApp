using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PersonalBlogApp.Enums;

namespace PersonalBlogApp.Models
{
    public class User : IdentityUser
    {
        [MaxLength(10)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }

        [MaxLength(150)]
        public string? AvatarUrl { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}