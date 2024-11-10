using ErrorOr;
using ImpoBooks.BusinessLogic.Errors.Users;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories.Users;
using ImpoBooks.Infrastructure;
using ImpoBooks.Infrastructure.Providers;

namespace ImpoBooks.BusinessLogic.Services;

public class UsersService(
    IUsersRepository usersRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : IUsersService
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<ErrorOr<Success>> RegisterAsync(User? user)
    {
        if (user is null)
            return UserErrors.IsNull;

        User dbUser = await _usersRepository.GetByEmailAsync(user.Email);
        if (dbUser is not null)
            return UserErrors.AlreadyExists;

        await _usersRepository.CreateAsync(user);
        return Result.Success;
    }

    public async Task<ErrorOr<string>> GenerateJwtAsync(string email, string password)
    {
        User dbUser = await _usersRepository.GetByEmailAsync(email);
        if (dbUser is null)
            return UserErrors.NotFoundByEmail;

        if (!_passwordHasher.Verify(password, dbUser.HashedPassword))
            return UserErrors.WrongPassword;
        
        return jwtProvider.GenerateToken(dbUser);
    }
}