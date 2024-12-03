using BlogApp.Application.Requests;
using BlogApp.Application.Responses;
using BlogApp.Domain.Base.Models;
using BlogApp.Domain.Models;

namespace BlogApp.Application.Interfaces;

public interface IPostService
{
    Task<(Response, Post?)> CreatePostAsync(PostCreateRequest postCreateRequest);

    Task<(Response, Post?)> UpdatePostAsync(PostUpdateRequest postUpdateRequest);

    Task<Response> DeletePostAsync(Guid id);

    Task<(Response, PostResponse?)> GetPostByIdAsync(Guid id);

    Task<(Response, IEnumerable<PostResponse?>)> GetAllPostsAsync();
}