namespace PersonalBlogApp.Requests
{
    public class PaginationRequest
    {
        public int draw { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 3;
        public string? searchvalue { get; set; }
    }
}
