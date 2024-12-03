namespace BlogApp.Application.Requests;

public class UserCreateRequest
{
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}