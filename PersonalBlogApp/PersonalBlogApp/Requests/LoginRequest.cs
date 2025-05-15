using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Please enter Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string PasswordHash { get; set; }
    }
}
