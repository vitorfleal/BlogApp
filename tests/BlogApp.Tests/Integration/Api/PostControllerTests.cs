using BlogApp.Application.Requests;
using BlogApp.Application.Responses;
using BlogApp.Domain.Base.Notifications;
using BlogApp.Domain.Models;
using BlogApp.Domain.Ports;
using BlogApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlogApp.Tests.Integration.Api;

public class PostControllerTests : IntegrationTestBase<Program>
{
    private readonly HttpClient _client;

    private readonly Guid _userId;

    public PostControllerTests()
    {
        _userId = Guid.NewGuid();

        var token = JwtTestHelper.GenerateToken(_userId);

        _client = GetTestAppClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact(DisplayName = "Should return status 'Created' when create a post valid")]
    public async Task CreatePost_CreatePostValid_ShouldReturnStatusCreated()
    {
        //Arrange
        var request = new PostCreateRequest()
        {
            Title = "Post create test",
            Content = "Post create test valid",
        };

        //Act
        var response = await _client.PostAsJsonAsync("/api/Post/Create", request);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();

        var postResponse = JsonConvert.DeserializeObject<PostResponse>(content);

        var postDb = await GetPostById(postResponse.Id);

        if (postDb is not null)
        {
            postDb.Title.Should().Be(request.Title);
            postDb.Content.Should().Be(request.Content);
            postDb.UserId.Should().Be(_userId);
        }
    }

    [Fact(DisplayName = "Should return status 'No Content' when update a post valid")]
    public async Task UpdatePost_UpdatePostValid_ShouldReturnStatusNoContent()
    {
        //Arrange
        var post = await GetFirstPost();

        var request = new PostUpdateRequest()
        {
            Id = post.Id,
            Title = "Post update test",
            Content = "Post update test valid",
        };

        //Act
        var response = await _client.PutAsJsonAsync("/api/Post/Update", request);

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var postDb = await GetPostById(request.Id);

        if (postDb is not null)
        {
            postDb.Title.Should().Be(request.Title);
            postDb.Content.Should().Be(request.Content);
            postDb.UserId.Should().Be(_userId);
        }
    }

    [Fact(DisplayName = "Should return status 'No Content' when remove a post valid")]
    public async Task RemovePost_RemovePostValid_ShouldReturnStatusNoContext()
    {
        //Arrange
        var post = await GetFirstPost();

        //Act
        var response = await _client.DeleteAsync($"/api/Post/Delete/{post.Id}");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var postDb = await GetPostById(post.Id);
        postDb.Should().BeNull();
    }

    [Fact(DisplayName = "Should return status 'Ok' when get a post valid")]
    public async Task GetPostById_GetPostValid_ShouldReturnStatusIsValid()
    {
        //Arrange
        var post = await GetFirstPost();

        //Act
        var response = await _client.GetAsync($"/api/Post/Find/{post.Id}");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var postResponse = JsonConvert.DeserializeObject<PostResponse>(content);

        postResponse.Should().NotBeNull();
        postResponse?.Title.Should().Be(post.Title);
        postResponse?.Content.Should().Be(post.Content);
    }

    [Fact(DisplayName = "Should return status 'Bad Request' when get a post invalid")]
    public async Task GetPostById_GetPostInValid_ShouldReturnStatusBadRequest()
    {
        //Arrange
        var id = Guid.Empty;

        //Act
        var response = await _client.GetAsync($"/api/Post/Find/{id}");

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("Post Id Invalid");
    }

    [Fact(DisplayName = "Should return status 'Not Found' when get a post invalid")]
    public async Task GetPostById_GetPostInValid_ShouldReturnStatusNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();

        //Act
        var response = await _client.GetAsync($"/api/Post/Find/{id}");

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response.Content.ReadAsStringAsync();

        var taskJobResponse = JsonConvert.DeserializeObject<Notification>(content);

        taskJobResponse.Should().NotBeNull();
        taskJobResponse?.Code.Should().Be(HttpStatusCode.NotFound);
        taskJobResponse?.Description.Should().Be("Post Not Found");
    }

    [Fact(DisplayName = "Should return status 'Ok' when get all posts valid")]
    public async Task GetAllPosts_GetAllPostsValid_ShouldReturnStatusIsValid()
    {
        //Arrange
        var post = await GetFirstPost();

        //Act
        var response = await _client.GetAsync("/api/Post/All");

        //Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var taskJobResponse = JsonConvert.DeserializeObject<IEnumerable<PostResponse>>(content);

        taskJobResponse.Should().NotBeNull();
        taskJobResponse.Should().HaveCount(1);
        taskJobResponse?.Select(x => x.Id).Should().Equal(post.Id);
        taskJobResponse?.Select(x => x.Title).Should().BeEquivalentTo(post.Title);
        taskJobResponse?.Select(x => x.Content).Should().BeEquivalentTo(post.Content);
    }

    private async Task<Post> GetFirstPost()
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return new();

        var postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();

        var post = await postRepository.GetAllAsync();

        return post.First();
    }

    private async Task<Post?> GetPostById(Guid id)
    {
        using var scope = ServiceProvider?.CreateScope();

        if (scope is null)
            return null;

        var postbRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();

        return await postbRepository.GetByIdAsync(id);
    }
}