using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories.Users;

namespace ImpoBooks.BusinessLogic.Services;

public class UsersService(IUsersRepository usersRepository) : IUsersService
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    
    public async Task CreateAsync(User user) => await _usersRepository.CreateAsync(user);
}