using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Infrastructure;
using ImpoBooks.Server.Requests;

namespace ImpoBooks.BusinessLogic.Extensions;

public static class UserExtensions
{
    private static readonly IPasswordHasher _passwordHasher = new PasswordHasher();
    public static User ToEntity(this RegisterUserRequest request)
    {
        return new User
        {
            Email = request.Email,
            Name = request.FullName,
            HashedPassword = _passwordHasher.Generate(request.Password)
        };
    }
}