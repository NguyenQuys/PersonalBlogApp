using PersonalBlogApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class UpdateUserRequest
    {
        [MaxLength(20)]
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public RoleEnum Role { get; set; }

        [MaxLength(150)]
        public string AvatarUrl { get; set; }
    }
}
