using Microsoft.AspNetCore.Mvc;

namespace PersonalBlogApp.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
