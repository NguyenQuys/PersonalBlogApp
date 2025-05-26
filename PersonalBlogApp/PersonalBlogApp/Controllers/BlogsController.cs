using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;
using PersonalBlogApp.Services;

namespace PersonalBlogApp.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService) => _blogService = blogService;

        // get all blogs for user
        public async Task<IActionResult> Index()
        {
            var result = await _blogService.GetAllAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Manage() => View();

        [HttpGet]
        public async Task<IActionResult> GetBlogsPagination([FromQuery] PaginationRequest request)
        {
            request.Start = (request.Start / request.Length) + 1;
     
            var userId = HttpContext.Items["UserId"]?.ToString();
            var isAdmin = HttpContext.Items["IsAdmin"] as bool? ?? false;

            request.CurrentUserId = userId;
            request.IsAdmin = isAdmin;

            var result = await _blogService.GetBlogsPagination(request);

            return Json(result);
        }

        // go to add blog view
        [HttpGet]
        public IActionResult Create() => View();
        

        // add blog
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BlogRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View(); 
            }

            var result = await _blogService.CreateAsync(request);
            return RedirectToAction("Details", new { id = result.Id });
        }

        // get detail
        [HttpGet("Blogs/{id:guid}")]
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Edit", new { id = request.Id });
                }

                var result = await _blogService.UpdateAsync(request);
                TempData["Success"] = "Edit blog successfully";
                return RedirectToAction("Details", new { id = request.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Edit", new { id = request.Id });
            }
        }

        [HttpDelete("Blogs/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _blogService.DeleteAsync(id);
            return Json(result);
        }
    }
}
