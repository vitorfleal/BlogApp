using BlogApp.Domain.Base.Models;

namespace BlogApp.Domain.Models;

public class User : Entity
{
    public string Name { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string PasswordHash { get; private set; }

    public List<Post> Posts { get; private set; }

    public User(string name, string username, string password, string passwordHash)
    {
        Name = name;
        Username = username;
        Password = password;
        PasswordHash = passwordHash;
    }

    public User()
    {
    }
}