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
    }
}
