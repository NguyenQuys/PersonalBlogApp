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

        public override async Task DeleteAsync(Guid id)
        {
            var parrentComment = await _dbSet.Include(c => c.Replies)
                                             .FirstOrDefaultAsync(c => c.Id == id);

            if(parrentComment != null)
            {
                _dbSet.RemoveRange(parrentComment.Replies);
                _dbSet.Remove(parrentComment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
