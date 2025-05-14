using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

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
            var register = await _authService.Register(request);
            return Json(register);
        }

        [HttpGet]
        public async Task<IActionResult> Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var result = await _authService.Login(request);
            return Json(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.Logout();
            return Json(result);
        }

        [HttpGet]
        public async Task <IActionResult> AccessDenied()
        {
            var result = await _authService.AccessDenied();
            return Json(result);
        }
    }
}
