using ErrorOr;
using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.BusinessLogic.Services.Auth;

public interface IAuthService
{
    Task<ErrorOr<Success>> RegisterAsync(User? user);
    Task<ErrorOr<string>> LoginAsync(string email, string password);
    ErrorOr<string> GenerateJwt(User? user);
}