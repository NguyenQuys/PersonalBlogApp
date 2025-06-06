﻿using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.DTOs;
using PersonalBlogApp.Models;
using PersonalBlogApp.Requests;
using PersonalBlogApp.Responses;

namespace PersonalBlogApp.Repositories
{
    public interface IUserRepository : IGenericsRepository<User>
    {
        Task<IEnumerable<User>> GetAllAsync(string? username);
        Task<PaginationResponse<UserDTO>> GetUsersPagination(PaginationRequest request);
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

        public async Task<PaginationResponse<UserDTO>> GetUsersPagination(PaginationRequest request)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(request.Searchvalue))
            {
                query = query.Where(m => m.UserName.Contains(request.Searchvalue) || m.Email.Contains(request.Searchvalue));
            }

            int recordsTotal = await query.CountAsync();

            int page = request.Start > 0 ? request.Start : 1;
            int pageSize = request.Length > 0 ? request.Length : 10;

            var data = await query
                .OrderByDescending(m => m.UserName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = data.Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Avatar = user.AvatarUrl,
            }).ToList();

            return new PaginationResponse<UserDTO>
            {
                Draw = request.Draw,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal,
                Data = result
            };
        }
    }
}