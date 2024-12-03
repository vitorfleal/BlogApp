using BlogApp.Application.Mapper;

namespace BlogApp.API.Extensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ConfiguringMapperProfile));

        return services;
    }
}