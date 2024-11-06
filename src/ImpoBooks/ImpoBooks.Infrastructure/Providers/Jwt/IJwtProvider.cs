using ImpoBooks.DataAccess.Entities;

namespace ImpoBooks.Infrastructure.Providers;

public interface IJwtProvider
{
    string GenerateToken(User user);
}