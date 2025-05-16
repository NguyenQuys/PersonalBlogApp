using Microsoft.AspNetCore.Identity;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;

namespace PersonalBlogApp.Services
{
    public interface IAuthService
    {
        Task<ApiResponse> Login(LoginRequest request);
        Task<ApiResponse> Register(UserRequest request);
        Task<ApiResponse> Logout();
        Task<ApiResponse> AccessDenied();
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApiResponse> Register(UserRequest request)
        {
            string fileName = "";
            List<string> errors = new List<string>();

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                AvatarUrl = "/images/avatar/" + request.AvatarUrl.FileName ?? ""
            };

            var result = await _userManager.CreateAsync(user, request.PasswordHash);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                if (request.AvatarUrl != null)
                {
                    fileName = await SaveImageFileAsync(request.AvatarUrl);
                }

                return new ApiResponse
                {
                    Status = 201,
                    Message = "Sign Up Successfully"
                };
            }
            else
            {
                foreach (var errorResult in result.Errors)
                {
                    errors.Add(errorResult.Description);
                }
                return new ApiResponse
                {
                    Status = 400,
                    //Message = string.Join("<br>", errors),
                    Errors = errors
                };
            }
        }

        public async Task<ApiResponse> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.PasswordHash))
                return new ApiResponse { Status = 400, Message = "Plese fill all of the input blank" };

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return new ApiResponse { Status = 400, Message = "User is not exist" };

            var result = await _signInManager.PasswordSignInAsync(user, request.PasswordHash, false, false);
            if (!result.Succeeded)
                return new ApiResponse { Status = 400, Message = "Wrong password" };

            return new ApiResponse { Status = 200, Message = "Login successfully" };
        }

        public async Task<ApiResponse> Logout()
        {
            await _signInManager.SignOutAsync();
            return new ApiResponse
            {
                Status = 200,
                Message = "Đăng xuất thành công"
            };
        }

        public async Task<ApiResponse> AccessDenied()
        {
            return new ApiResponse
            {
                Status = 401,
                Message = "Bạn không có quyền truy cập vào chức năng này"
            };
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
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể lưu file. Lỗi: " + ex.Message);
            }

            return "/images/avatar/" + fileName;
        }
    }
}
