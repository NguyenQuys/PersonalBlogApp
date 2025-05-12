using Microsoft.AspNetCore.Mvc;

namespace PersonalBlogApp.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
