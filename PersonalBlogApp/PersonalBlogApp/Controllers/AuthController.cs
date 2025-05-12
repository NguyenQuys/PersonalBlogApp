using Microsoft.AspNetCore.Mvc;

namespace PersonalBlogApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
