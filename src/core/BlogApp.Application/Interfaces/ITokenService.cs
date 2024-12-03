using BlogApp.Domain.Models;

namespace BlogApp.Application.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User user);
}