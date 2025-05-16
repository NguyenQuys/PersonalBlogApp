using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;
using PersonalBlogApp.Requests;

namespace PersonalBlogApp.Services
{
    public interface IBlogService : IGenericsService<Blog> {

        Task<IEnumerable<Blog>> GetByUserId(string userId);
        Task<string> CreateAsync(BlogRequest request);
        Task<string> UpdateAsync(BlogRequest request);
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

        public async Task<string> CreateAsync(BlogRequest request)
        {
            var newBlog = new Blog
            {
                Title = request.Title,
                Content = request.Content,
                UserId = request.UserId,
                Priority = request.Priority
            };

            await _blogRepository.CreateAsync(newBlog);
            return "Create blog successfully";
        }

        public Task<string> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            var result = await _blogRepository.GetAllAsync();
            return result;
        }

        public async Task<IEnumerable<Blog>> GetByUserId(string userId)
        {
            var result = await _blogRepository.GetByUserId(userId);
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


        public async Task<string> UpdateAsync(BlogRequest request)
        {
                
                var blogOwner = await _userManager.FindByIdAsync(request.UserId);
            return "";
        }
    }
}
