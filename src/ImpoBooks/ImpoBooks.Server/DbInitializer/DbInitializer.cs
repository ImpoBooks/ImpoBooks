using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.Infrastructure;

namespace ImpoBooks.DataAccess.Entities;

using System.Threading.Tasks;

public class DbInitializer(
    IUsersRepository usersRepository,
    IConfiguration configuration,
    IPasswordHasher passwordHasher)
    : IDbInitializer
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task SeedAsync()
    {
        var admins = _configuration.GetSection("Admins").Get<IEnumerable<User>>();

        foreach (var admin in admins)
        {
            User existingUser = await _usersRepository.GetByEmailAsync(admin.Email);

            if (existingUser is null)
            {
                admin.HashedPassword = passwordHasher.Generate(admin.HashedPassword);
                admin.RoleId = 1; //Admin
                await _usersRepository.CreateAsync(admin);
            }
        }
    }
}