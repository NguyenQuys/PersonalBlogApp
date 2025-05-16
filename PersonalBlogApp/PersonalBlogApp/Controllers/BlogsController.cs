using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Services;
using System.Security.Claims;

namespace PersonalBlogApp.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
       {
            object result;
            if (User.IsInRole("Admin"))
            {
                result = await _blogService.GetAllAsync();
            }
            else
            {
                result = await _blogService.GetByUserId(User.FindFirst(ClaimTypes.NameIdentifier).ToString());
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BlogRequest request)
        {
            var result = await _blogService.CreateAsync(request);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _blogService.GetByIdAsync(id);
            if (result == null) {
                TempData["Error"] = "This blog is not exist";
                return RedirectToAction("GetAll");
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _blogService.GetByIdAsync(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] BlogRequest request)
        {
            //var userRole = await User.FindFirst
            //var result = await _blogService.UpdateAsync(request);
            //return RedirectToAction("Edit", new {id = request.Id});
            return View();
        }
  
    }
}
