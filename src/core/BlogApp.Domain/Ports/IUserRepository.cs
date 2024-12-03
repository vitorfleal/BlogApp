using BlogApp.Domain.Models;

namespace BlogApp.Domain.Ports;

public interface IUserRepository
{
    Task AddAsync(User user);

    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByIdAsync(Guid userId);
}