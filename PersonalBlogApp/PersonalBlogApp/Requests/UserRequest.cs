using PersonalBlogApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class UserRequest 
    {
        public string PasswordHash { get; set; }
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }

        public double? LockoutTime { get; set; }
        public IFormFile? AvatarUrl { get; set; }

        [Required(ErrorMessage = "User has at least one role")]
        public IList<string>? Roles { get; set; } = new List<string> { "User" };
    }
}
