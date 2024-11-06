using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Infrastructure;
using ImpoBooks.Server.DTOs;

namespace ImpoBooks.BusinessLogic.Extensions;

public static class UserExtensions
{
    private static readonly IPasswordHasher _passwordHasher = new PasswordHasher();
    public static User ToEntity(this CreateUserDto dto)
    {
        return new User
        {
            Email = dto.Email,
            Name = dto.FullName,
            HashedPassword = _passwordHasher.Generate(dto.Password)
        };
    }
}