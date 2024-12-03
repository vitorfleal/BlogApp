using BlogApp.Domain.Models;
using BlogApp.Domain.Ports;
using BlogApp.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbSet<User> _user;

    public UserRepository(BlogAppContext dbContext)
    {
        _user = dbContext.Users;
    }

    public async Task AddAsync(User user)
    {
        await _user.AddAsync(user);
    }

    public async Task<User?> GetByIdAsync(Guid userId) => await _user.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

    public async Task<User?> GetByUsernameAsync(string username) => await _user.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
}