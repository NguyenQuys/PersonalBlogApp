namespace PersonalBlogApp.Responses
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public object Result { get; set; }
        public List<string>? Errors { get; set; } = new();
    }
}
