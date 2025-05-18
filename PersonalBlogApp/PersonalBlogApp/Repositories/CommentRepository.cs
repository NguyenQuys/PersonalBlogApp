using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Repositories
{
    public interface ICommentRepository : IGenericsRepository<Comment>
    {
    }

    public class CommentRepository : GenericsRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Comment> CreateAsync(Comment entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return await _dbSet.Include(m => m.User)
                               .FirstOrDefaultAsync(m => m.Id == entity.Id);
        }
    }
}
