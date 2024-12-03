using BlogApp.Domain.Ports;
using BlogApp.Infra.Data.Contexts;

namespace BlogApp.Infra.Data.Peersistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlogAppContext _context;

    public UnitOfWork(BlogAppContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}