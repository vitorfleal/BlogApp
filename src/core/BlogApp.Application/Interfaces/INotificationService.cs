using BlogApp.Domain.Models;

namespace BlogApp.Application.Interfaces;

public interface INotificationService
{
    Task NotifyNewPost(Post post);
}