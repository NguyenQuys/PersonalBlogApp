using PersonalBlogApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class UserRequest
    {
        [MaxLength(20)]
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(150)]
        public IFormFile? AvatarUrl { get; set; }
    }
}
