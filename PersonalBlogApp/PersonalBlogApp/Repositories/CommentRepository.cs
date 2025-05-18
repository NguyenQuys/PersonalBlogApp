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
    }
}
