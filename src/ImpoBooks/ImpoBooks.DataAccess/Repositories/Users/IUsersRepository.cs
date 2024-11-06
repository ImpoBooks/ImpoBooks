using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.DataAccess.Repositories.Users;

public interface IUsersRepository
{
    Task CreateAsync(User user);
}