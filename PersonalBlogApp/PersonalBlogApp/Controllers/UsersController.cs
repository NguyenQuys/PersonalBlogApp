using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;
using PersonalBlogApp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalBlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users(string searchValue, string roleValue) // send params form search and filter 
        {
            var getAllUsers = await _userService.GetAllAsync(searchValue, roleValue);
            return View(getAllUsers);
        }

        [HttpGet("Users/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var result = await _userService.GetUser(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var userModel = await _userService.GetUser(id);
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromForm] DetailUserResponse userRequest,List<string> rolesSelected)
        {
            var result = await _userService.UpdateAsync(userRequest.User,rolesSelected);
            if(result.Status == 201)
            {
                TempData["SuccessMessage"] = "User updated successfully";
                return RedirectToAction("Details", new { id = userRequest.User.Id });
            }
            else
            {
                if (result.Result is IEnumerable<string> errors)
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                var user = await _userService.GetUser(userRequest.User.Id);

                return View(user); 
            }
        }

        [HttpDelete("Users/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _userService.DeleteAsync(id);
            return Json("Deleted successfully");
        }
    }
}
