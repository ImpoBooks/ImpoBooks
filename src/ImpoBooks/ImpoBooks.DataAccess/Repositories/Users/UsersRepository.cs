using ImpoBooks.DataAccess.Entities;
using Supabase;

namespace ImpoBooks.DataAccess.Repositories.Users;

public class UsersRepository(Client client) : IUsersRepository
{
    private readonly Client _client = client;

    public async Task CreateAsync(User user) => await _client.From<User>().Insert(user);
}