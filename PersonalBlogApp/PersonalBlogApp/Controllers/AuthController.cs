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
            var result = await _authService.Register(request);
            if (result.Status != 201)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(request);

            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var result = await _authService.Login(request);
            if (result.Status != 200)
            { 
               ModelState.AddModelError(string.Empty, result.Message);              
               return View(request);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.Logout();
            return Json(result);
        }

        [HttpGet]
        public async Task <IActionResult> AccessDenied()
        {
            //var result = await _authService.AccessDenied();
            //return Json(result);
            return View();
        }
    }
}
