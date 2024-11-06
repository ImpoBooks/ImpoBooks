using ErrorOr;
using ImpoBooks.BusinessLogic.Errors.Users;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories.Users;

namespace ImpoBooks.BusinessLogic.Services;

public class UsersService(IUsersRepository usersRepository) : IUsersService
{
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<ErrorOr<Success>> CreateAsync(User user)
    {
        if (user is null) return UserErrors.IsNull;
        
        User dbUser = await _usersRepository.GetBtEmailAsync(user.Email);
        if (dbUser is not null) return UserErrors.AlreadyExists;
        
        await _usersRepository.CreateAsync(user);
        return Result.Success;
    }
}