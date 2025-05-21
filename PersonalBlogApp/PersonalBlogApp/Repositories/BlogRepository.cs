using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Repositories
{
    public interface IBlogRepository : IGenericsRepository<Blog>
    {
        Task<IEnumerable<Blog>> GetBlogs();
        Task<IEnumerable<Blog>> SortAndFilter(string sortValue,int prioriyValue, string userId);
    }

    public class BlogRepository : GenericsRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Blog>> SortAndFilter(string sortValue, int prioriyValue, string userId)
        {
            var query = _dbSet.AsQueryable();

            if (prioriyValue != 0)
            {
                query = query.Where(m => m.Priority == prioriyValue);
            }

            if (string.IsNullOrEmpty(sortValue) || sortValue.Equals("newest"))
            {
                query = query.OrderByDescending(m => m.CreatedDate);
            }
            else if (sortValue.Equals("oldest"))
            {
                query = query.OrderBy(m => m.CreatedDate);
            }

            if (userId != null)
            {
                query = query.Where(m=>m.UserId == userId);
            }

            return await query.ToListAsync();
        }

        public override async Task<Blog> GetByIdAsync(Guid id)
        {
            var blog = await _dbSet
                .Include(m => m.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (blog != null && blog.Comments != null)
            {
                blog.Comments = blog.Comments
                    .OrderByDescending(c => c.CreatedDate)
                    .ToList();
            }

            return blog;
        }

        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await _dbSet.Where(m=>m.IsPublic)
                               .OrderByDescending(m => m.CreatedDate)
                               .ToListAsync();
        }
    }
}
