using BlogApp.Domain.Base.Models;

namespace BlogApp.Domain.Models;

public class Post : Entity
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public Guid UserId { get; private set; }

    public User User { get; set; }

    public Post(string title, string content, Guid userId)
    {
        Title = title;
        Content = content;
        UserId = userId;
    }

    public Post()
    {
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
    }
}