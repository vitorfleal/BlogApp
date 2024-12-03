using System.Net;

namespace BlogApp.Domain.Base.Notifications;

public class Notification
{
    public Notification(HttpStatusCode code, string description)
    {
        Code = code;
        Description = description;
    }

    public HttpStatusCode Code { get; }
    public string Description { get; }

    public override string ToString()
    {
        return $"{Code}: {Description}";
    }
}