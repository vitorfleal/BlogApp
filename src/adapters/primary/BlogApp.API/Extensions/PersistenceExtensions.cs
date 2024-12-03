using BlogApp.Domain.Ports;
using BlogApp.Infra.Data.Contexts;
using BlogApp.Infra.Data.Peersistence;
using BlogApp.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.API.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        if (!env.IsEnvironment("Testing"))
        {
            services.AddDbContext<BlogAppContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"), builder =>
                    builder.MigrationsAssembly("BlogApp.Infra.Data"))
                );
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IPostRepository, PostRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        return services;
    }
}