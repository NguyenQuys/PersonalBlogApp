using Microsoft.AspNetCore.Identity;
using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;
using PersonalBlogApp.Requests;

namespace PersonalBlogApp.Services
{
    public interface ICommentService 
    {
        Task<Comment> Create(CommentRequest request);
        Task Delete(Guid id);
    }

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;

        public CommentService(ICommentRepository commentRepository, UserManager<User> userManager)
        {
            _commentRepository = commentRepository;
            _userManager = userManager;
        }

        public async Task<Comment> Create(CommentRequest request)
        {
            var newComment = new Comment
            {
                Content = request.Content,
                UserId = request.UserId,
                BlogId = request.BlogId,
            };

            var result = await _commentRepository.CreateAsync(newComment);
            return result;
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
