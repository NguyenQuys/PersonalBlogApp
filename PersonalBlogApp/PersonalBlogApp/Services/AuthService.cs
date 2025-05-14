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
            if (request.AvatarUrl != null)
            {
                fileName = await SaveImageFileAsync(request.AvatarUrl);
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                AvatarUrl = fileName
            };

            var result = await _userManager.CreateAsync(user, request.PasswordHash);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return new ApiResponse
                {
                    Status = 201,
                    Message = "Đăng ký thành công"
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
            string error = "";
            if (request.UserName == null || request.PasswordHash == null)
            {
                error = "Không được để trống";
            }

            var exstingUser = await _userManager.FindByNameAsync(request.UserName);

            if (exstingUser == null) {
                error = "Không tồn tại user này";
            }

            var result = await _signInManager.PasswordSignInAsync(exstingUser, request.PasswordHash, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded) {
                error = "Sai mật khẩu. Vui lòng nhập lại";
            }

            if(error == null)
            {
                return new ApiResponse
                {
                    Status = 400,
                    Message = error
                };
            }
            else
            {
                return new ApiResponse
                {
                    Status = 200,
                    Message = "Đăng nhập thành công"
                };
            }
          
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
