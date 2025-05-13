namespace PersonalBlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
