using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Repositories
{
    public interface IBlogRepository : IGenericsRepository<Blog>
    {
        Task<IEnumerable<Blog>> GetByUserId(string userId);
        Task<IEnumerable<Blog>> SortAndFilter(string sortValue,int prioriyValue);
    }

    public class BlogRepository : GenericsRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Blog>> GetByUserId(string userId) => await _dbSet.Where(m=>m.UserId.Equals(userId)).ToListAsync();

        //public async Task<IEnumerable<Blog>> Sort(string sortValue)
        //{
        //    //if(sortValue == "newest")
        //    //{
        //    //    return await _dbSet.OrderByDescending(m=>m.CreatedDate).ToListAsync();
        //    //}
        //    //return await _dbSet.OrderBy(m=>m.CreatedDate).ToListAsync();
        //}

        public async Task<IEnumerable<Blog>> SortAndFilter(string sortValue, int prioriyValue)
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

            return await query.ToListAsync();
        }

    }
}
