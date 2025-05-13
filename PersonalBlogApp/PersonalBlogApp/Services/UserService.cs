using PersonalBlogApp.Models;
using PersonalBlogApp.Repositories;

namespace PersonalBlogApp.Services
{
    public interface IUserService : IGenericsService<User>
    {

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<string> CreateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var result = await _userRepository.GetAllAsync();
            return result;
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
