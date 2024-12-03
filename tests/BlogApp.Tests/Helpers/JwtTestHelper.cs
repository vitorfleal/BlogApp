using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Tests.Helpers;

public class JwtTestHelper
{
    private const string SecretKey = "2fe82c11550ee9bbfdd79bc82803bcc6";
    public static string GenerateToken(Guid userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, "Test User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}