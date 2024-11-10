using System.Text;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Infrastructure;
using ImpoBooks.Server.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ImpoBooks.BusinessLogic.Extensions;

public static class UserExtensions
{
    private static readonly IPasswordHasher _passwordHasher = new PasswordHasher();

    public static User? ToEntity(this RegisterUserRequest request)
    {
        return new User
        {
            Email = request.Email,
            Name = request.FullName,
            HashedPassword = _passwordHasher.Generate(request.Password)
        };
    }

    public static void AddApiAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["necessary-cookies"];
                        if (!string.IsNullOrEmpty(token)) context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });
        serviceCollection.AddAuthorization();
    }
}