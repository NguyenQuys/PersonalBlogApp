namespace PersonalBlogApp.Requests
{
    public class PaginationRequest
    {
        public int Draw { get; set; }
        public int Index { get; set; } = 1;
        public int PageSize { get; set; } = 2;
        public string? Searchvalue { get; set; }
    }
}
