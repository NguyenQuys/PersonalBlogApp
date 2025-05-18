using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Services;

namespace PersonalBlogApp.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CommentRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.UserId = userId;

            var result = await _commentService.Create(request);
            return Json(result);
        }
    }
}
