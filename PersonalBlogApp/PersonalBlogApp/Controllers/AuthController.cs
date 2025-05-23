using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Security.Claims;

namespace PersonalBlogApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _authService.Register(request);
            if (result.Status != 201)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(request);
            }
            else
            {
                TempData["Success"] = result.Result.ToString();
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public  IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _authService.Login(request);
            if (result.Status != 200)
            { 
               ModelState.AddModelError(string.Empty, result.Result.ToString());              
               return View(request);
            }
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.Logout();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                request.userId = userId;

                var result = await _authService.ChangePassword(request);
                TempData["Success"] = result;
                return View();
            } catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            //var result = await _authService.AccessDenied();
            //return Json(result);
            return View();
        }
    }
}
