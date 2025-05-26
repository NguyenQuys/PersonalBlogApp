using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.DTOs;
using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;

namespace PersonalBlogApp.Services
{
    public interface IBlogService : IGenericsService<Blog> {

        Task<PaginationResponse<BlogDTO>> GetBlogsPagination(PaginationRequest request);
        Task<Blog> CreateAsync(BlogRequest request);
        Task<Blog> UpdateAsync(BlogRequest request);
        //Task<IEnumerable<Blog>> SortAndFilter(string sortValue,int prioriryValue,string userId);

    }

    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly UserManager<User> _userManager;

        public BlogService(IBlogRepository blogRepository, UserManager<User> userManager)
        {
            _blogRepository = blogRepository;
            _userManager = userManager;
        }

        public async Task<Blog> CreateAsync(BlogRequest request)
        {
            var newBlog = new Blog
            {
                Title = request.Title,
                Content = request.Content,
                UserId = request.UserId,
                Priority = request.Priority,
                IsPublic = request.IsPublic
            };

            var result = await _blogRepository.CreateAsync(newBlog);
            return result;
        }

        public async Task<string> DeleteAsync(Guid id)
        {
            await _blogRepository.DeleteAsync(id);
            return "Delete blog successfully";
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            var result = await _blogRepository.GetBlogs();
            foreach (var user in result)
            {
                user.User = await _userManager.FindByIdAsync(user.UserId);
            }
            return result;
        }

        public async Task<PaginationResponse<BlogDTO>> GetBlogsPagination(PaginationRequest request)
        {
            request.Searchvalue = request.Searchvalue?.Trim() ?? null;
            var result = await _blogRepository.GetBlogsPagination(request);
      
            var userIds = result.Data.Select(b => b.UserId).Distinct().ToList();

            var users = await _userManager.Users
                                          .Where(u => userIds.Contains(u.Id))
                                          .ToDictionaryAsync(u => u.Id, u => u.UserName);

            foreach (var blog in result.Data)
            {


                var userName = "Unknown";
                users.TryGetValue(blog.UserId, out userName);
                blog.UserName = userName;


                blog.Actions = $@"
                                <div class='dropdown'>
                                    <button class='btn btn-link dropdown-toggle' type='button' id='blogActionsDropdown-{blog.Id}' data-bs-toggle='dropdown' aria-expanded='false'>
                                        More
                                    </button>
                                    <ul class='dropdown-menu' aria-labelledby='blogActionsDropdown-{blog.Id}'>
                                        <li><a class='dropdown-item' href='/Blogs/{blog.Id}'>Details</a></li>";


                if (request.CurrentUserId.Equals(blog.UserId))
                {
                    blog.Actions += $@"
                                        <li><a class='dropdown-item' href='/Blogs/Edit/{blog.Id}'>Edit</a></li>
                                    ";
                }

                if (request.IsAdmin || request.CurrentUserId.Equals(blog.UserId))
                {
                    blog.Actions += $@"
                                        <li><a class='dropdown-item' style='cursor:pointer' onclick=DeleteBlog('{blog.Id}')>Delete</a></li>
                                    ";
                }
            }

            return result;
        }

        public async Task<Blog> GetByIdAsync(Guid id)
        {
            var result = await _blogRepository.GetByIdAsync(id);

            if (result != null && !string.IsNullOrEmpty(result.UserId))
            {
                result.User = await _userManager.FindByIdAsync(result.UserId);
            }
            return result;
        }

        public async Task<Blog> UpdateAsync(BlogRequest request)
        {    
            var blogOwner = await _userManager.FindByIdAsync(request.UserId);

            if (!blogOwner.Id.Equals(request.UserId))
            {
                throw new Exception("You don't have permession to edit");
            }

            var blogToUpdate = new Blog
            {
                Id = (Guid)request.Id,
                Title = request.Title,
                Content = request.Content,
                UserId = request.UserId,
                User = await _userManager.FindByIdAsync(request.UserId),
                Priority = request.Priority,
                IsPublic = request.IsPublic
            };

            var result = await _blogRepository.UpdateAsync(blogToUpdate);
            return result;
        }
    }
}
