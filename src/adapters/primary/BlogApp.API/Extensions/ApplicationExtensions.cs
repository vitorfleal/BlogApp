using BlogApp.Application.Interfaces;
using BlogApp.Application.Services;

namespace BlogApp.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IPostService, PostService>();
        services.AddTransient<ITokenService, TokenService>();

        services.AddSingleton<INotificationService, NotificationService>();

        return services;
    }
}