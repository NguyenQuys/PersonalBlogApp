using Microsoft.AspNetCore.Mvc;

namespace PersonalBlogApp.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
