using AutoMapper;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Requests;
using BlogApp.Application.Responses;
using BlogApp.Domain.Base.Models;
using BlogApp.Domain.Models;
using BlogApp.Domain.Ports;
using System.Net;

namespace BlogApp.Application.Services;

public class PostService : AppService, IPostService
{
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly INotificationService _notificationService;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper, IPostRepository postRepository, INotificationService notificationService) : base(unitOfWork)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _notificationService = notificationService;
    }

    public async Task<(Response, Post?)> CreatePostAsync(PostCreateRequest postCreateRequest)
    {
        var post = new Post(postCreateRequest.Title, postCreateRequest.Content, postCreateRequest.UserId);

        try
        {
            await _postRepository.AddAsync(post);

            await Commit();

            _notificationService.NotifyNewPost(post);

            return (Response.Valid(), post);
        }
        catch (Exception ex)
        {
            return (Response.Invalid(HttpStatusCode.InternalServerError, ex.Message), post);
        }
    }

    public async Task<Response> DeletePostAsync(Guid id)
    {
        try
        {
            var post = await _postRepository.GetByIdAsync(id);

            if (post is null)
                return Response.Invalid(HttpStatusCode.BadRequest, "Post not found.");

            _postRepository.Delete(post);

            await Commit();

            return Response.Valid();
        }
        catch (Exception ex)
        {
            return Response.Invalid(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<(Response, Post?)> UpdatePostAsync(PostUpdateRequest postUpdateRequest)
    {
        var post = await _postRepository.GetByIdAsync(postUpdateRequest.Id);

        try
        {
            if (post is null)
                return (Response.Invalid(HttpStatusCode.BadRequest, "Post not found."), post);

            var postMapper = _mapper.Map(postUpdateRequest, post);

            _postRepository.Update(postMapper);

            await Commit();

            return (Response.Valid(), post);
        }
        catch (Exception ex)
        {
            return (Response.Invalid(HttpStatusCode.InternalServerError, ex.Message), post);
        }
    }

    public async Task<(Response, PostResponse?)> GetPostByIdAsync(Guid id) => (Response.Valid(), _mapper.Map<PostResponse?>(await _postRepository.GetByIdAsync(id)));

    public async Task<(Response, IEnumerable<PostResponse?>)> GetAllPostsAsync() => (Response.Valid(), _mapper.Map<IEnumerable<PostResponse?>>(await _postRepository.GetAllAsync()));
}