using BlogApp.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace BlogApp.API.Extensions;

public static class ValidationsExtension
{
    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssemblyContaining<PostCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<PostUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserCreateValidator>();

        return services;
    }
}