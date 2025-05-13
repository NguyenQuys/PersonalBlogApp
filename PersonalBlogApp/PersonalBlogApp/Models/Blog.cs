using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        [Range(1,5, ErrorMessage ="Độ ưu tiên chỉ trong khoảng từ 1 đến 5")]
        public int Priority { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
