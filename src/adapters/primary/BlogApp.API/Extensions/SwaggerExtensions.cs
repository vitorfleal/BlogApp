using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BlogApp.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BlogApp API",
                Version = "v1",
                Description = "API para gerenciamento de postagens de blog com autenticação JWT e notificações em tempo real.",
            });

            // Configuração de segurança para autenticação JWT na UI do Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Description = "Entre com o token JWT no formato `Bearer <token>`"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddFluentValidationRulesToSwagger();

        return services;
    }
}