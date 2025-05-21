using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;
using PersonalBlogApp.Requests;

namespace PersonalBlogApp.Services
{
    public interface IBlogService : IGenericsService<Blog> {

        //Task<IEnumerable<Blog>> GetBlogs();
        Task<Blog> CreateAsync(BlogRequest request);
        Task<Blog> UpdateAsync(BlogRequest request);
        Task<IEnumerable<Blog>> SortAndFilter(string sortValue,int prioriryValue,string userId);
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

        public async Task<Blog> GetByIdAsync(Guid id)
        {
            var result = await _blogRepository.GetByIdAsync(id);

            if (result != null && !string.IsNullOrEmpty(result.UserId))
            {
                result.User = await _userManager.FindByIdAsync(result.UserId);
            }
            return result;
        }

        public async Task<IEnumerable<Blog>> SortAndFilter(string sortBy, int priority, string currentUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            IList<string> currentUserRoles = new List<string>();

            var isCurrentUserAdmin = false;
            if (currentUser != null)
            {
                currentUserRoles = await _userManager.GetRolesAsync(currentUser);
                if (currentUserRoles.Contains("Admin"))
                {
                    isCurrentUserAdmin = true;
                }
            }

            IEnumerable<Blog> blogs = new List<Blog>();
            if (isCurrentUserAdmin)
            {
                blogs = await _blogRepository.SortAndFilter(sortBy, priority, null);
            }
            else
            {
                blogs = await _blogRepository.SortAndFilter(sortBy, priority, currentUserId);
            }

            foreach (var blog in blogs)
            {
                blog.User = await _userManager.FindByIdAsync(blog.UserId);
            }
            return blogs;
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
