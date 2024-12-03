namespace BlogApp.Application.Requests;

public class PostUpdateRequest : PostCreateRequest
{
    public Guid Id { get; set; }
}