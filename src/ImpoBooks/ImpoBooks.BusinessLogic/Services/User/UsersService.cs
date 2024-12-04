using ErrorOr;
using ImpoBooks.BusinessLogic.Errors.Users;
using ImpoBooks.BusinessLogic.Services;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.Infrastructure;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;


    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<User>> UpdateUserAsync(int userId, string name, string password)
    {
        var user = await _usersRepository.GetByIdAsync(userId);
        if (user is null) return UserErrors.IsNull;
        user.RoleId = 2;//Customer

        if (!string.IsNullOrWhiteSpace(name)) user.Name = name;
        if (!string.IsNullOrWhiteSpace(password))
            user.HashedPassword = _passwordHasher.Generate(password);

        await _usersRepository.UpdateAsync(user);
        return user;
    }


    public async Task<ErrorOr<Success>> DeleteUserAsync(int userId)
    {
        User user = await _usersRepository.GetByIdAsync(userId);
        if (user is null) return UserErrors.IsNull;
        await _usersRepository.DeleteAsync(user);
        return Result.Success;
    }
}