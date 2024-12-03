using FluentValidation;
using System.Net;

namespace BlogApp.Domain.Base.Notifications;

public abstract class Notifiable
{
    private readonly List<Notification> _notifications = new List<Notification>();

    public abstract bool IsValid();

    public bool HasNotifications =>
        _notifications.Any();

    public IEnumerable<Notification> GetNotifications() =>
        _notifications.AsReadOnly();

    public void AddNotification(HttpStatusCode code, string description) =>
        _notifications.Add(new Notification(code, description));

    protected bool Validate<T>(IValidator<T> validator)
    {
        var context = new ValidationContext<object>(this);
        var result = validator.Validate(context);

        foreach (var item in result.Errors)
        {
            if (Enum.TryParse(item.PropertyName, out HttpStatusCode statusCode))
                AddNotification(statusCode, item.ErrorMessage);
        }

        return !HasNotifications;
    }
}