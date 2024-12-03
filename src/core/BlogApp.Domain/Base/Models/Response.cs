using BlogApp.Domain.Base.Notifications;
using System.Net;

namespace BlogApp.Domain.Base.Models;

public class Response
{
    private readonly List<Notification> _notifications = new();

    public Response()
    {
    }

    public Response(IEnumerable<Notification> notificaions)
    {
        AddNotifications(notificaions);
    }

    public Response(HttpStatusCode code, string description)
    {
        AddNotification(code, description);
    }

    public Response(Notification notification)
    {
        AddNotification(notification);
    }

    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

    public void AddNotification(HttpStatusCode code, string description) =>
        _notifications.Add(new Notification(code, description));

    public void AddNotification(Notification notification) =>
        _notifications.Add(notification);

    public void AddNotifications(IEnumerable<Notification> notifications) =>
        _notifications.AddRange(notifications);

    public ResponseErrors ToValidationErrors() =>
        new("VALIDATION_ERRORS", _notifications.AsReadOnly());

    public bool IsValid() => !_notifications.Any();

    public static Response Valid() => new();

    public static Response Invalid(IEnumerable<Notification> notifications) => new(notifications);

    public static Response Invalid(Notification notification) => new(notification);

    public static Response Invalid(HttpStatusCode code, string description) => new(code, description);
}

public class ResponseErrors
{
    public ResponseErrors(string type, IReadOnlyCollection<Notification> notifications)
    {
        Type = type;
        Notifications = notifications;
    }

    public string Type { get; private set; }
    public IReadOnlyCollection<Notification> Notifications { get; private set; }
}