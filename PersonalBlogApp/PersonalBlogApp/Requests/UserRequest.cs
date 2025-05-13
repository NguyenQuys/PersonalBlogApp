using PersonalBlogApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class UserRequest
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public IFormFile? AvatarUrl { get; set; }
    }
}
