using BlogApp.Domain.Models;

namespace BlogApp.Domain.Ports;

public interface IPostRepository
{
    Task AddAsync(Post post);

    Task<Post?> GetByIdAsync(Guid id);

    Task<IEnumerable<Post?>> GetAllAsync();

    void Update(Post post);

    void Delete(Post post);
}