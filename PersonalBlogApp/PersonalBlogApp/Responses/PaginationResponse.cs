namespace PersonalBlogApp.Responses
{
    public class PaginationResponse<T>
    {
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
