using BlogApp.Application.Requests;
using BlogApp.Domain.Base.Models;

namespace BlogApp.Application.Interfaces;

public interface IAuthService
{
    Task<(Response, string)> RegisterAsync(UserCreateRequest userRegisterRequest);

    Task<(Response, string)> LoginAsync(UserLoginRequest userLoginRequest);
}