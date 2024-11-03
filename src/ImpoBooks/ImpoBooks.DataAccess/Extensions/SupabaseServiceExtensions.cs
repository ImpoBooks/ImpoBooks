using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Supabase;

namespace ImpoBooks.DataAccess;

public static class SupabaseServiceExtensions
{
    public static IServiceCollection AddSupabaseClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSingleton<Supabase.Client>(x => new Supabase.Client(
            configuration["Supabase:Url"],
            configuration["Supabase:Key"],
            new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
            }));
    }
}