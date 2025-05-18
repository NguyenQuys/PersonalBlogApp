using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;

namespace PersonalBlogApp.Services
{
    public interface IUserService 
    {
        Task<IEnumerable<User>> GetAllAsync(string? username, string? roleValue);
        Task<DetailUserResponse> GetByIdAsync(string id);
        Task<ApiResponse> UpdateAsync(UserRequest entity,List<string> rolesSelected);
        Task<string> DeleteAsync(string id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(existingUser);
    
            return "Delete successfully";
        }

        public async Task<IEnumerable<User>> GetAllAsync(string? username, string? roleValue)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                users = users.Where(u => u.UserName.Contains(username));
            }

            var userList = await users.ToListAsync();

            if (!string.IsNullOrEmpty(roleValue))
            {
                var filteredUsers = new List<User>();
                foreach (var user in userList)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains(roleValue, StringComparer.OrdinalIgnoreCase))
                    {
                        filteredUsers.Add(user);
                    }
                }
                return filteredUsers;
            }
            else
            {
                return userList;
            }
        }


        public async Task<DetailUserResponse> GetByIdAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            var active = existingUser.LockoutEnd != null ? 1 : 0;
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }
            var roles = await _userManager.GetRolesAsync(existingUser);

            var user = new UserRequest
            {
                Id = id,
                UserName = existingUser.UserName,
                Email = existingUser.Email,
                Avatar = existingUser.AvatarUrl,
                Roles = roles,
                LockoutTime = active
            };

            return new DetailUserResponse
            {
                User = user,
                //UserRoles = roles,
                AllRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync()
            };
        }

        public async Task<ApiResponse> UpdateAsync(UserRequest request, List<string> rolesSelected)
        {
            var existingUser = await _userManager.FindByIdAsync(request.Id);

            existingUser.UserName = request.UserName;
            existingUser.Email = request.Email;
            if (request.AvatarUrl != null)
            {
                var avatarFileName = await SaveImageFileAsync(request.AvatarUrl);
                existingUser.AvatarUrl = avatarFileName;
            }

            if(request.LockoutTime != null)
            {
                existingUser.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(request.LockoutTime.Value);
            }
            else
            {
                existingUser.LockoutEnd = null;
            }

            if (rolesSelected.Count == 0)
            {
                return new ApiResponse
                {
                    Status = 400,
                    Result = "Every user has at least one role"
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(existingUser);

            var removeResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

            var addResult = await _userManager.AddToRolesAsync(existingUser, rolesSelected);

            var updateResult = await _userManager.UpdateAsync(existingUser);

            if (updateResult.Succeeded && addResult.Succeeded && removeResult.Succeeded)
            {
                return new ApiResponse
                {
                    Status = 201,
                    Result = "Update successfully"
                };
            }
            else
            {
                var errors = updateResult.Errors.Select(e => e.Description).ToList();
                errors.AddRange(addResult.Errors.Select(e => e.Description));
                errors.AddRange(removeResult.Errors.Select(e => e.Description));
                throw new Exception(string.Join(", ", errors));
            }
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
