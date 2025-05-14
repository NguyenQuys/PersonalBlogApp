using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> Register([FromForm] UserRequest request)
        {
            var register = await _authService.Register(request);
            return Json(register);
        }

        [HttpGet]
        public async Task<IActionResult> Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var result = await _authService.Login(request);
            return Json(result);
        }
    }
}
