using BlogApp.API;
using BlogApp.API.Extensions;
using BlogApp.Application.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddAutoMapper();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration, builder.Environment);
builder.Services.AddSwagger();
builder.Services.AddSignalR();
builder.Services.AddValidations();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogApp API V1");
        c.RoutePrefix = string.Empty;
    });
    app.UseDeveloperExceptionPage();
}

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.MapHub<NotificationHub>("/notificationHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

Task.Run(async () => await SignalRTestClient.StartClient());

app.Run();

public partial class Program
{ }