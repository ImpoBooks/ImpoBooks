using ErrorOr;
using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.BusinessLogic.Services;

public interface IUsersService
{
    Task<ErrorOr<Success>> RegisterAsync(User? user);
    Task<ErrorOr<string>> GenerateJwtAsync(string email, string password);
}