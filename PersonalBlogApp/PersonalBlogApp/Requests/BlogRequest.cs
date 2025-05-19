using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Requests
{
    public class BlogRequest
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Title is not empty")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is not empty")]
        public string Content { get; set; }
        public int Priority { get; set; }
        public string UserId { get; set; }
        public string? UserRole { get; set; }
    }
}
