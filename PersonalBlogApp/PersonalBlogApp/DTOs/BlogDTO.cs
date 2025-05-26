namespace PersonalBlogApp.DTOs
{
    public class BlogDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Actions { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
