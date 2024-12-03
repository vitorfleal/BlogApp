namespace BlogApp.Application.Responses;

public class PostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedDate { get; set; }
}