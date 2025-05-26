using Microsoft.AspNetCore.Mvc;

namespace PersonalBlogApp.Requests
{
    public class PaginationRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        [FromQuery(Name = "search[value]")]
        public string? Searchvalue { get; set; }
        public string? CurrentUserId { get; set; }
        public bool IsAdmin { get; set; }

        [FromQuery(Name = "order[0][column]")]
        public int OrderColumn { get; set; }

        [FromQuery(Name = "order[0][dir]")]
        public string OrderDir { get; set; }

        [FromQuery(Name = "columns[0][data]")]
        public string Col0 { get; set; }

        [FromQuery(Name = "columns[1][data]")]
        public string Col1 { get; set; }

        [FromQuery(Name = "columns[2][data]")]
        public string Col2 { get; set; }

        [FromQuery(Name = "columns[3][data]")]
        public string Col3 { get; set; }

        [FromQuery(Name = "columns[4][data]")]
        public string Col4 { get; set; }

        [FromQuery(Name = "columns[5][data]")]
        public string Col5 { get; set; }
    }
}
