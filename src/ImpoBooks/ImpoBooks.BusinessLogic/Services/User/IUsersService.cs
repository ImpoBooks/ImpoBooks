using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.BusinessLogic.Services;

public interface IUsersService
{
    Task CreateAsync(User user);
}