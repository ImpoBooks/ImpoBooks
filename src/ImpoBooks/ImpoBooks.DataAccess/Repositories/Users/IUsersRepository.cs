using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.DataAccess.Repositories.Users;

public interface IUsersRepository
{
    Task CreateAsync(User? user);
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmailAsync(string email);
}