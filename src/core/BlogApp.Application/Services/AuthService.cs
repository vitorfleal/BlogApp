using BlogApp.Application.Interfaces;
using BlogApp.Application.Requests;
using BlogApp.Domain.Base.Models;
using BlogApp.Domain.Models;
using BlogApp.Domain.Ports;
using System.Net;

namespace BlogApp.Application.Services;

public class AuthService : AppService, IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork unitOfWork, IUserRepository userRepository, ITokenService tokenService) : base(unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<(Response, string)> RegisterAsync(UserCreateRequest userRegisterRequest)
    {
        try
        {
            var existinUser = await _userRepository.GetByUsernameAsync(userRegisterRequest.Username);

            if (existinUser != null)
                return (Response.Invalid(HttpStatusCode.BadRequest, "User already exists."), string.Empty);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterRequest.Password);

            var user = new User(userRegisterRequest.Name, userRegisterRequest.Username, userRegisterRequest.Password, passwordHash);

            await _userRepository.AddAsync(user);

            await Commit();

            return (Response.Valid(), _tokenService.GenerateJwtToken(user));
        }
        catch (Exception ex)
        {
            return (Response.Invalid(HttpStatusCode.InternalServerError, ex.Message), default);
        }
    }

    public async Task<(Response, string)> LoginAsync(UserLoginRequest userLoginRequest)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(userLoginRequest.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginRequest.Password, user.PasswordHash))
                return (Response.Invalid(HttpStatusCode.Unauthorized, "Invalid credentials."), string.Empty);

            return (Response.Valid(), _tokenService.GenerateJwtToken(user));
        }
        catch (Exception ex)
        {
            return (Response.Invalid(HttpStatusCode.InternalServerError, ex.Message), default);
        }
    }
}