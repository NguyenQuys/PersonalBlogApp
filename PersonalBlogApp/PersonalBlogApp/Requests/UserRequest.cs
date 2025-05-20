using PersonalBlogApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class UserRequest 
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Firstname is not empty")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is not empty")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Username is not empty")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is not empty")]
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string? Avatar { get; set; }

        public double? LockoutTime { get; set; }
        public IFormFile? AvatarUrl { get; set; }

        [Required(ErrorMessage = "User has at least one role")]
        public IList<string>? Roles { get; set; } = new List<string> { "User" };
    }
}
