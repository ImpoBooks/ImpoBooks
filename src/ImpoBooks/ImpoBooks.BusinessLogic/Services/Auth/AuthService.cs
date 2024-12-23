using ErrorOr;
using ImpoBooks.BusinessLogic.Errors.Users;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.Infrastructure;
using ImpoBooks.Infrastructure.Providers;

namespace ImpoBooks.BusinessLogic.Services.Auth;

public class AuthService(
    IUsersRepository usersRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : IAuthService
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<ErrorOr<Success>> RegisterAsync(User? user)
    {
        if (user is null)
            return UserErrors.IsNull;

        if (string.IsNullOrWhiteSpace(user.Email))
            return UserErrors.EmailIsNullOrEmpty;

        User dbUser = await _usersRepository.GetByEmailAsync(user.Email);
        if (dbUser is not null)
            return UserErrors.AlreadyExists;

        await _usersRepository.CreateAsync(user);
        return Result.Success;
    }

    public async Task<ErrorOr<string>> LoginAsync(string email, string password)
    {
        User dbUser = await _usersRepository.GetByEmailAsync(email);
        if (dbUser is null)
            return UserErrors.NotFoundByEmail;

        if (!_passwordHasher.Verify(password, dbUser.HashedPassword))
            return UserErrors.WrongPassword;

        return jwtProvider.GenerateToken(dbUser);
    }


    public ErrorOr<string> GenerateJwt(User dbUser)
    {
        if (dbUser is null)
            return UserErrors.IsNull;

        return jwtProvider.GenerateToken(dbUser);
    }
}