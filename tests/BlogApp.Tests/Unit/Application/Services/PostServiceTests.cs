using AutoMapper;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Requests;
using BlogApp.Application.Responses;
using BlogApp.Application.Services;
using BlogApp.Domain.Models;
using BlogApp.Domain.Ports;
using FluentAssertions;
using Moq;

namespace BlogApp.Tests.Unit.Application.Services;

public class PostServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPostRepository> _mockPostRepository;
    private readonly Mock<INotificationService> _mockNotificationService;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockPostRepository = new Mock<IPostRepository>();
        _mockNotificationService = new Mock<INotificationService>();

        _postService = new PostService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockPostRepository.Object,
            _mockNotificationService.Object
        );
    }

    [Fact]
    public async Task CreatePostAsync_ShouldReturnValidResponse_WhenPostIsCreatedSuccessfully()
    {
        // Arrange
        var postCreateRequest = new PostCreateRequest
        {
            Title = "Test Title",
            Content = "Test Content",
            UserId = Guid.NewGuid()
        };

        _mockPostRepository.Setup(x => x.AddAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.Commit()).Returns(Task.CompletedTask);
        _mockNotificationService.Setup(n => n.NotifyNewPost(It.IsAny<Post>())).Verifiable();

        // Act
        var (response, createdPost) = await _postService.CreatePostAsync(postCreateRequest);

        // Assert
        response.IsValid().Should().BeTrue();
        createdPost.Should().NotBeNull();
        createdPost.Title.Should().Be(postCreateRequest.Title);
        createdPost.Content.Should().Be(postCreateRequest.Content);
        createdPost.UserId.Should().Be(postCreateRequest.UserId);
        _mockPostRepository.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);
        _mockNotificationService.Verify(n => n.NotifyNewPost(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task CreatePostAsync_ShouldReturnInvalidResponse_WhenExceptionIsThrown()
    {
        // Arrange
        var postCreateRequest = new PostCreateRequest
        {
            Title = "Test Title",
            Content = "Test Content",
            UserId = Guid.NewGuid()
        };

        _mockPostRepository.Setup(x => x.AddAsync(It.IsAny<Post>())).ThrowsAsync(new Exception("Some error"));

        // Act
        var (response, createdPost) = await _postService.CreatePostAsync(postCreateRequest);

        // Assert
        response.IsValid().Should().BeFalse();        
        response.Notifications.Should().Contain(n => n.Description == "Some error");
        createdPost.Should().NotBeNull();
        _mockPostRepository.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_ShouldReturnValidResponse_WhenPostIsUpdatedSuccessfully()
    {
        // Arrange
        var postUpdateRequest = new PostUpdateRequest
        {
            Id = Guid.NewGuid(),
            Title = "Updated Title",
            Content = "Updated Content",
            UserId = Guid.NewGuid()
        };

        var existingPost = new Post("Old Title", "Old Content", postUpdateRequest.UserId);

        _mockPostRepository.Setup(x => x.GetByIdAsync(postUpdateRequest.Id)).ReturnsAsync(existingPost);
        _mockMapper.Setup(m => m.Map(It.IsAny<PostUpdateRequest>(), It.IsAny<Post>())).Returns(existingPost);
        _mockPostRepository.Setup(x => x.Update(It.IsAny<Post>())).Verifiable();
        _mockUnitOfWork.Setup(u => u.Commit()).Returns(Task.CompletedTask);

        // Act
        var (response, updatedPost) = await _postService.UpdatePostAsync(postUpdateRequest);

        // Assert
        response.IsValid().Should().BeTrue();
        updatedPost.Should().NotBeNull();
        updatedPost.Title.Should().Be(existingPost.Title);
        updatedPost.Content.Should().Be(existingPost.Content);
        _mockPostRepository.Verify(x => x.Update(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_ShouldReturnInvalidResponse_WhenPostNotFound()
    {
        // Arrange
        var postUpdateRequest = new PostUpdateRequest
        {
            Id = Guid.NewGuid(),
            Title = "Updated Title",
            Content = "Updated Content",
            UserId = Guid.NewGuid()
        };

        _mockPostRepository.Setup(x => x.GetByIdAsync(postUpdateRequest.Id)).ReturnsAsync((Post?)null);

        // Act
        var (response, post) = await _postService.UpdatePostAsync(postUpdateRequest);

        // Assert
        response.IsValid().Should().BeFalse();
        response.Notifications.Should().Contain(n => n.Description == "Post not found.");
    }


    [Fact]
    public async Task DeletePostAsync_ShouldReturnValidResponse_WhenPostIsDeleted()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var post = new Post("Test Title", "Test Content", Guid.NewGuid());

        _mockPostRepository.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);
        _mockPostRepository.Setup(x => x.Delete(It.IsAny<Post>())).Verifiable();
        _mockUnitOfWork.Setup(u => u.Commit()).Returns(Task.CompletedTask);

        // Act
        var response = await _postService.DeletePostAsync(postId);

        // Assert
        response.IsValid().Should().BeTrue();
        _mockPostRepository.Verify(x => x.Delete(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task DeletePostAsync_ShouldReturnInvalidResponse_WhenPostNotFound()
    {
        // Arrange
        var postId = Guid.NewGuid();

        _mockPostRepository.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync((Post?)null);

        // Act
        var response = await _postService.DeletePostAsync(postId);

        // Assert
        response.IsValid().Should().BeFalse();
        response.Notifications.Should().Contain(n => n.Description == "Post not found.");
    }

    [Fact]
    public async Task GetPostByIdAsync_ShouldReturnValidResponse_WhenPostIsFound()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var post = new Post("Test Title", "Test Content", Guid.NewGuid());
        var postResponse = new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content
        };

        _mockPostRepository.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);
        _mockMapper.Setup(m => m.Map<PostResponse>(It.IsAny<Post>())).Returns(postResponse);

        // Act
        var (response, result) = await _postService.GetPostByIdAsync(postId);

        // Assert
        response.IsValid().Should().BeTrue();
        result.Should().NotBeNull();
        result.Id.Should().Be(post.Id);
    }

    [Fact]
    public async Task GetPostByIdAsync_ShouldReturnInvalidResponse_WhenPostIsNotFound()
    {
        // Arrange
        var postId = Guid.NewGuid();
        _mockPostRepository.Setup(x => x.GetByIdAsync(postId)).ReturnsAsync((Post?)null);

        // Act
        var (response, result) = await _postService.GetPostByIdAsync(postId);

        // Assert
        response.IsValid().Should().BeTrue();
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllPostsAsync_ShouldReturnValidResponse_WhenPostsAreFound()
    {
        // Arrange
        var posts = new List<Post>
        {
            new("Test Title 1", "Test Content 1", Guid.NewGuid()),
            new("Test Title 2", "Test Content 2", Guid.NewGuid())
        };

        var postResponses = posts.Select(post => new PostResponse
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content
        }).ToList();

        _mockPostRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(posts);
        _mockMapper.Setup(m => m.Map<IEnumerable<PostResponse>>(It.IsAny<IEnumerable<Post>>())).Returns(postResponses);

        // Act
        var (response, result) = await _postService.GetAllPostsAsync();

        // Assert
        response.IsValid().Should().BeTrue();
        result.Should().HaveCount(2);
    }
}

