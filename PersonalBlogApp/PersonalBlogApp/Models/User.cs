using System.ComponentModel.DataAnnotations;
using PersonalBlogApp.Enums;

namespace PersonalBlogApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string Email {  get; set; }

        public RoleEnum Role { get; set; }

        [MaxLength(150)]
        public string AvatarUrl { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
