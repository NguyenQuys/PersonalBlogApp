namespace PersonalBlogApp.Requests
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid BlogId { get; set; }
    }
}
