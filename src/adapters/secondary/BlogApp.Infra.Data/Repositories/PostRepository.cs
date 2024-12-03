using BlogApp.Domain.Models;
using BlogApp.Domain.Ports;
using BlogApp.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infra.Data.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DbSet<Post> _post;

    public PostRepository(BlogAppContext dbContext)
    {
        _post = dbContext.Posts;
    }

    public async Task AddAsync(Post post)
    {
        await _post.AddAsync(post);
    }

    public void Delete(Post post)
    {
        _post.Remove(post);
    }

    public async Task<IEnumerable<Post?>> GetAllAsync() => await _post.AsNoTracking().ToListAsync();

    public async Task<Post?> GetByIdAsync(Guid id) => await _post.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public void Update(Post post)
    {
        _post.Update(post);
    }
}