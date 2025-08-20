using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicStream.Application.Interfaces.Auth;

namespace MusicStream.Infrastructure.Auth;

internal class TokenGenerator(IOptions<JwtOption> jwtOption) : ITokenGenerator
{
    private readonly JwtOption option = jwtOption.Value;

    public string JwtToken(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.Secret));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: option.Issuer,
            audience: option.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(option.ExpirationInMinutes),
            signingCredentials: credential
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string RefreshToken()
    {
        throw new NotImplementedException();
    }
}
