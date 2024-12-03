using System.Text.Json.Serialization;

namespace BlogApp.Application.Requests;

public class PostCreateRequest
{
    public string Title { get; set; }
    public string Content { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}