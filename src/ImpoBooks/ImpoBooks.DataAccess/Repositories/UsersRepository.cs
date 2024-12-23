using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Interfaces;
using Supabase;
using Supabase.Postgrest.Responses;

namespace ImpoBooks.DataAccess.Repositories;

public class UsersRepository(Client client) : Repository<User, AutoIncUser>(client), IUsersRepository
{
    private readonly Client _client = client;
    
    public async Task<User> GetByEmailAsync(string email)
    {
        ModeledResponse<User> response = await _client.From<User>().Where(x => x.Email == email).Get();
        User user = response.Model;
        return user;
    }
    
    public async Task<User> UpdateAsync(User user)
    {
        ModeledResponse<User> response = await _client.From<User>().Update(user);
        return response.Model;
    }
}