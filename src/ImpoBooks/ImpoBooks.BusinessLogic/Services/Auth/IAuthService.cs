using ErrorOr;
using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.BusinessLogic.Services.Auth;

public interface IAuthService
{
    Task<ErrorOr<Success>> RegisterAsync(User? user);
    Task<ErrorOr<string>> GenerateJwtAsync(string email, string password);
}