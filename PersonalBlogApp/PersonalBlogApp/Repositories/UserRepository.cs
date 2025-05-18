using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Repositories
{
    public interface IUserRepository : IGenericsRepository<User>
    {
        Task<IEnumerable<User>> GetAllAsync(string? username);
    }

    public class UserRepository : GenericsRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetAllAsync(string? username)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(u => u.UserName.Contains(username));

            return await query.ToListAsync();
        }

    }
}