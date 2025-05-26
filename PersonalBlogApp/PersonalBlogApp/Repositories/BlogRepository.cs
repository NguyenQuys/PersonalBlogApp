using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;
using PersonalBlogApp.DTOs;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;

namespace PersonalBlogApp.Repositories
{
    public interface IBlogRepository : IGenericsRepository<Blog>
    {
        Task<IEnumerable<Blog>> GetBlogs();
        //Task<IEnumerable<Blog>> SortAndFilter(string sortValue,int prioriyValue, string userId);
        Task<PaginationResponse<BlogDTO>> GetBlogsPagination(PaginationRequest request);
    }

    public class BlogRepository : GenericsRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
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

        public async Task<PaginationResponse<BlogDTO>> GetBlogsPagination(PaginationRequest request)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(request.Searchvalue))
            {
                query = query.Where(m => m.Title.Contains(request.Searchvalue) 
                                      || m.Content.Contains(request.Searchvalue) 
                                      || m.User.UserName.Contains(request.Searchvalue)
                                      || m.Priority.ToString().Contains(request.Searchvalue));
            }

            if (!request.IsAdmin)
            {
                query = query.Where(m => m.UserId.Equals(request.CurrentUserId));
            }

            int recordsTotal = await query.CountAsync();

            int page = request.Start > 0 ? request.Start : 1;
            int pageSize = request.Length > 0 ? request.Length : 10;

            var data = await query
                .OrderByDescending(m => m.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = data.Select(item => new BlogDTO
            {
                Id = item.Id,
                Title = item.Title,
                Content = item.Content,
                Priority = item.Priority,
                CreatedDate = item.CreatedDate,
                UserId = item.UserId
            }).ToList();

            return new PaginationResponse<BlogDTO>
            {
                Draw = request.Draw,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal,
                Data = result
            };
        }
    }
}
