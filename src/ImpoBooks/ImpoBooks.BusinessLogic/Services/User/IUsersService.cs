using ErrorOr;
using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.BusinessLogic.Services;

public interface IUsersService
{
    Task<ErrorOr<Success>> CreateAsync(User user);
}