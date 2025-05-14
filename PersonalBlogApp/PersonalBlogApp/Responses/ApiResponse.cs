namespace PersonalBlogApp.Responses
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<string>? Errors { get; set; } = new();
    }
}
