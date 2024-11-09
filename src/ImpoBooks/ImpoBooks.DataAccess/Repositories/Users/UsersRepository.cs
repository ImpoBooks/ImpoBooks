using ImpoBooks.DataAccess.Entities;
using Supabase;
using Supabase.Postgrest.Responses;

namespace ImpoBooks.DataAccess.Repositories.Users;

public class UsersRepository(Client client) : IUsersRepository
{
    private readonly Client _client = client;

    public async Task CreateAsync(User? user) => await _client.From<User>().Insert(user);

    public async Task<User> GetByIdAsync(int id) 
    {
        ModeledResponse<User> response = await _client.From<User>().Where(x => x.Id == id).Get();
        User user = response.Model;
        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        ModeledResponse<User> response = await _client.From<User>().Where(x => x.Email == email).Get();
        User user = response.Model;
        return user;
        
    }
}