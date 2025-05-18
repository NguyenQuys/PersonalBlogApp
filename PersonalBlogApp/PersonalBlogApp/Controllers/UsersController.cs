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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers(string? username, string? roleValue) // send params form search and filter 
        {
            var getAllUsers = await _userService.GetAllAsync(username, roleValue);
            return View(getAllUsers);
        }

        public async Task<IActionResult> Details(string id)
        {
            var result = await _userService.GetByIdAsync(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var userModel = await _userService.GetByIdAsync(id);
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
                ModelState.AddModelError(string.Empty, result.Result.ToString());
                var user = await _userService.GetByIdAsync(userRequest.User.Id);

                return View(user); 
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _userService.GetByIdAsync(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmDelete(string id)
        {
            var result = await _userService.DeleteAsync(id);
            TempData["SuccessMessage"] = "User deleted successfully";
            return RedirectToAction("GetAllUsers");
        }

        [HttpPost]
        public async Task<IActionResult> SearchByUsername([FromForm] string username)
        {
            return RedirectToAction("GetAllUsers", new { username });
        }

        [HttpPost]
        public async Task<IActionResult> FilterByRole([FromForm] string roleValue)
        {
            return RedirectToAction("GetAllUsers", new {  roleValue });
        }
    }
}
