using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Repositories
{
    public interface IBlogRepository : IGenericsRepository<Blog>
    {
        Task<IEnumerable<Blog>> GetByUserId(string userId);
    }

    public class BlogRepository : GenericsRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Blog>> GetByUserId(string userId) => await _dbSet.Where(m=>m.UserId.Equals(userId)).ToListAsync();
        
    }
}
