using BlogApp.Application.Hubs;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlogApp.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyNewPost(Post post)
    {
        Console.WriteLine($"[Server] Enviando notificação de novo post: {post.Title}");

        await _hubContext.Clients.All.SendAsync("ReceiveNotification", post.Title, post.Content);
    }
}