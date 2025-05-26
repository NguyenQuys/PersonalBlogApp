using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.DTOs;
using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;

namespace PersonalBlogApp.Services
{
    public interface IUserService 
    {
        Task<PaginationResponse<UserDTO>> GetUsersPagination(PaginationRequest request);
        Task<IEnumerable<DetailUserResponse>> GetAllAsync(string? searchValue, string? roleValue);
        Task<DetailUserResponse> GetUser(string id);
        Task<ApiResponse> UpdateAsync(UserRequest entity,List<string> rolesSelected);
        Task DeleteAsync(string id);
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

        public async Task DeleteAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(existingUser);
        }

        public async Task<IEnumerable<DetailUserResponse>> GetAllAsync(string? searchValue, string? roleValue)
        {
            var usersQuery = _userManager.Users.AsQueryable();
            List<User> users;

            if (!string.IsNullOrEmpty(searchValue))
            {
                users = await usersQuery.Where(u => u.UserName.Contains(searchValue)).ToListAsync();

                if (users.Count == 0)
                {
                    users = await usersQuery.Where(u => u.Email.Contains(searchValue)).ToListAsync();
                }
            }
            else
            {
                users = await usersQuery.ToListAsync();
            }

            var result = new List<DetailUserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (!string.IsNullOrEmpty(roleValue) && !roles.Contains(roleValue, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                var userRequest = new UserRequest
                {
                    Id = user.Id,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Avatar = user.AvatarUrl,
                    Roles = roles
                };

                result.Add(new DetailUserResponse
                {
                    User = userRequest,
                    UserRoles = roles
                });
            }

            return result;
        }

        public async Task<PaginationResponse<UserDTO>> GetUsersPagination(PaginationRequest request)
        {
            request.Searchvalue = request.Searchvalue?.Trim() ?? null;
            var result = await _userRepository.GetUsersPagination(request);
            foreach (var user in result.Data)
            {
                user.Avatar = $@"
                                   <img src='{user.Avatar}' height='70' width='70' class='rounded-circle border border-primary'>
                                ";

                user.Actions = $@"
                                <div class='dropdown'>
                                    <button class='btn btn-link dropdown-toggle' type='button' id='userActionsDropdown-{user.Id}' data-bs-toggle='dropdown' aria-expanded='false'>
                                        More
                                    </button>
                                    <ul class='dropdown-menu' aria-labelledby='userActionsDropdown-{user.Id}'>
                                        <li><a class='dropdown-item' href='/Users/{user.Id}'>Details</a></li>
";
                if (user.Id.Equals(request.CurrentUserId) || request.IsAdmin)
                {
                    user.Actions += $@"
                                        <li><a class='dropdown-item' href='/Users/Edit/{user.Id}'>Edit</a></li>
                                    ";
                }

                if(user.Id.Equals(request.CurrentUserId) || request.IsAdmin)
                {
                    user.Actions += $@"
                                        <li><a class='dropdown-item' style='cursor:pointer' onclick=DeleteUser('{user.Id}')>Delete</a></li>
                                    ";
                }
            }
            return result;
        }

        public async Task<DetailUserResponse> GetUser(string id)
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
                LockoutTime = active,
                Firstname = existingUser.FirstName,
                Lastname = existingUser.LastName
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
            if(string.IsNullOrEmpty(request.Firstname) || string.IsNullOrEmpty(request.Lastname))
            {
                return new ApiResponse
                {
                    Status = 400,
                    Result = new List<string> { "First name and last name are required." }
                };
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new ApiResponse
                {
                    Status = 400,
                    Result = new List<string> { "Email is required." }
                };
            }

            var existingUser = await _userManager.FindByIdAsync(request.Id);

            // update user
            existingUser.UserName = request.UserName;
            existingUser.FirstName = request.Firstname;
            existingUser.LastName = request.Lastname;
            existingUser.Email = request.Email;
            if (request.AvatarUrl != null)
            {
                var avatarFileName = await SaveImageFileAsync(request.AvatarUrl);
                existingUser.AvatarUrl = avatarFileName;
            }

            if (request.LockoutTime != null)
                existingUser.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(request.LockoutTime.Value);
            else
                existingUser.LockoutEnd = null;

            if (rolesSelected.Count == 0)
            {
                return new ApiResponse
                {
                    Status = 400,
                    Result = new List<string> { "Every user has at least one role" }
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(existingUser);
            var rolesToAdd = rolesSelected.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(rolesSelected).ToList();

            var removeResult = IdentityResult.Success;
            if (rolesToRemove.Any())
                removeResult = await _userManager.RemoveFromRolesAsync(existingUser, rolesToRemove);

            var addResult = IdentityResult.Success;
            if (rolesToAdd.Any())
                addResult = await _userManager.AddToRolesAsync(existingUser, rolesToAdd);

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
                var errors = updateResult.Errors.Select(e => e.Description)
                    .Concat(addResult.Errors.Select(e => e.Description))
                    .Concat(removeResult.Errors.Select(e => e.Description))
                    .Distinct()
                    .ToList();

                return new ApiResponse
                {
                    Status = 400,
                    Result = errors
                };
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
