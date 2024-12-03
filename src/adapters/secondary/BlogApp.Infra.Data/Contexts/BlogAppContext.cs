using BlogApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace BlogApp.Infra.Data.Contexts;

public class BlogAppContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }

    public BlogAppContext(DbContextOptions<BlogAppContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}