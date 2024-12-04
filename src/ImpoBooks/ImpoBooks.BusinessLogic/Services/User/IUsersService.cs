using ErrorOr;
using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.BusinessLogic.Services;

public interface IUsersService
{
    Task<ErrorOr<User>> UpdateUserAsync(int userId, string name, string password);
    Task<ErrorOr<Success>> DeleteUserAsync(int userId);

}