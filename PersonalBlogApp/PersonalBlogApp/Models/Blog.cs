using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    public class Blog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        public User User { get; set; }

        [Range(1,5, ErrorMessage ="Priority in range 1 - 5")]
        public int Priority { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
