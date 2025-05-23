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

        // get all blogs for admin
        //[HttpGet("Blogs/Manage")]
        //public async Task<IActionResult> Blogs(string sortValue, int priorityValue)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        TempData["Error"] = "User is not authenticated.";
        //        return RedirectToAction("Index");
        //    }

        //    var result = await _blogService.SortAndFilter(sortValue, priorityValue, userId);
        //    return View(result);
        //}
        [HttpGet]
        [Authorize(Policy = "CanViewAllBlogs")]
        public async Task<IActionResult> Manage() => View();

        [HttpGet]
        public async Task<IActionResult> GetBlogsPagination()
        {
            var draw = Request.Query["draw"].FirstOrDefault();
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            var searchValue = Request.Query["search[value]"].FirstOrDefault();

            int drawInt = 0;
            int.TryParse(draw, out drawInt);

            int skip = 0;
            int.TryParse(start, out skip);

            int pageSize = 10; 
            int.TryParse(length, out pageSize);

            var request = new PaginationRequest
            {
                Draw = drawInt,
                Index = (skip / pageSize) + 1,
                PageSize = pageSize,
                Searchvalue = searchValue,
            };

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
