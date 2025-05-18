using Microsoft.AspNetCore.Identity;
using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;

namespace PersonalBlogApp.Services
{
    public interface ICommentService 
    {
        Task<ApiResponse> Create(CommentRequest request);
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

        public async Task<ApiResponse> Create(CommentRequest request)
        {
            var newComment = new Comment
            {
                Content = request.Content,
                UserId = request.UserId,
                BlogId = request.BlogId,
            };

            var result = await _commentRepository.CreateAsync(newComment);

            return new ApiResponse
            {
                Status = 201,
                Result = new
                {
                    Content = newComment.Content,
                    Username = result.User.UserName,
                    createdDate = result.CreatedDate,
                }
            };
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
