using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.DataAccess.Interfaces;

public interface IUsersRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
}