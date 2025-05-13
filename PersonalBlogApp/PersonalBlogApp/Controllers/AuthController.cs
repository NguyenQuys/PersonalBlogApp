using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;

namespace PersonalBlogApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRequest request)
        {
            string fileName = "";
            if (request.AvatarUrl != null) {
                fileName = await SaveImageFileAsync(request.AvatarUrl); 
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                AvatarUrl = fileName
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded) {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        private async Task<string> SaveImageFileAsync(IFormFile file)
        {
            var folderPath = Path.Combine("wwwroot/images/avatar");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                if (Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            } catch (Exception ex) {
                throw new Exception("Không thể lưu file. Lỗi: " + ex.Message);
            }

            return "/images/avatar" + fileName;
        }
    }
}
