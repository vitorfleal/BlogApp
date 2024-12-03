using Microsoft.AspNetCore.SignalR;

namespace BlogApp.Application.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string postTitle, string postContent)
    {
        await Clients.All.SendAsync("ReceiveNotification", postTitle, postContent);
    }
}