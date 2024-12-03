using BlogApp.Domain.Models;

namespace BlogApp.Infra.Data.Contexts;

public class BlogAppDatabaseSeed
{
    private readonly BlogAppContext _context;

    public BlogAppDatabaseSeed(BlogAppContext context)
    {
        _context = context;
    }

    public void SeedData()
    {
        SeedTaskJob();
    }


    private void SeedTaskJob()
    {
        if (!_context.Posts.Any())
        {
            var taskJob = new Post("Post test 1", "Post test description 1", Guid.NewGuid());

            _context.Posts.AddRange(taskJob);
            _context.SaveChanges();
        }
    }
}
