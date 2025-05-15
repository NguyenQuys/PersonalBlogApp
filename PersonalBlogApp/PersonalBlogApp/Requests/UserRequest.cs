using PersonalBlogApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class UserRequest : UserBaseRequest
    {
        public string PasswordHash { get; set; }
    }

    public class UserBaseRequest
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public IFormFile? AvatarUrl { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
