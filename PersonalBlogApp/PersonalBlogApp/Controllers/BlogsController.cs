using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Services;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Scripting;

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
        public async Task<IActionResult> GetAll(string sortValue, int priority)
        {
            var result = await _blogService.SortAndFilter(sortValue, priority);
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
            return RedirectToAction("Details", new { id = result.Id });
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

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _blogService.GetByIdAsync(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var result = await _blogService.DeleteAsync(id);
            TempData["Success"] = result;
            return RedirectToAction("GetAll");
        }

        [HttpPost]
        public async Task<IActionResult> SortAndFilter(string sortValue,int priorityValue)
        {
            var result = await _blogService.SortAndFilter(sortValue, priorityValue);
            return RedirectToAction("GetAll", new {sortValue = sortValue, priority  = priorityValue});
        }
    }
}
