using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class ChangePasswordRequest
    {
        public string? userId { get; set; }
        [Required(ErrorMessage = "OldPassword is not empty")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "NewPassword is not empty")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is not empty")]
        public string ConfirmNewPassword { get; set; }
    }
}
