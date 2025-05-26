namespace PersonalBlogApp.Models
{
    public class Like
    {
        public Guid Id { get; set; } = new Guid();

        public string UserId { get; set; }
        public User Users { get; set; }

        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}
