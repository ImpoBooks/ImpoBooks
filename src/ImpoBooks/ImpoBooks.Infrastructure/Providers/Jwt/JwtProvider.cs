using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImpoBooks.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ImpoBooks.Infrastructure.Providers;

public class JwtProvider(IConfiguration configuration) : IJwtProvider
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(User user)
    {
        Claim[] claims = [new Claim("userId", user.Id.ToString()), new Claim("email", user.Email)]; 
        
        SigningCredentials signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtOptions:SecretKey"])),
                SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddHours(_configuration.GetValue<int>("JwtOptions:ExpiresHours"))
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}