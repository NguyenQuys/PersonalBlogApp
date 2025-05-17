using PersonalBlogApp.Repositories;

namespace PersonalBlogApp.Services
{
    public interface IGenericsService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid id);
    }
}
