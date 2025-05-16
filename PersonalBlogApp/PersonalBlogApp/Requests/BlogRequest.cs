namespace PersonalBlogApp.Requests
{
    public class BlogRequest
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Priority { get; set; }
        public string UserId { get; set; }
        public string? UserRole { get; set; }
    }
}
