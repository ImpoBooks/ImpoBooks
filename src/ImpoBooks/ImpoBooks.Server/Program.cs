using ImpoBooks.BusinessLogic.Extensions;
using ImpoBooks.BusinessLogic.Services;
using ImpoBooks.BusinessLogic.Services.Auth;
using ImpoBooks.BusinessLogic.Services.Cart;
using ImpoBooks.BusinessLogic.Services.Catalog;
using ImpoBooks.BusinessLogic.Services.Product;
using ImpoBooks.DataAccess;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure;
using ImpoBooks.Infrastructure.Providers;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Middleware;

public class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", true, true);
        builder.Services.AddRepository();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
        builder.Services.AddSingleton<IUsersService, UsersService>();
        builder.Services.AddSingleton<ICatalogService, CatalogService>();
        builder.Services.AddSingleton<IProductService, ProductService>();
		builder.Services.AddSingleton<ICartService, CartService>();
		builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
        builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
        builder.Services.AddSingleton<IDbInitializer, DbInitializer>();
        builder.Services.AddSupabaseClient(builder.Configuration);
        builder.Services.AddApiAuthentication(builder.Configuration);
        builder.Services.AddExceptionHandler<GlobalExeptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("admin"));
        });
        builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.AddRouting(options => { options.LowercaseUrls = true; });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            //Open CORS
            options.AddPolicy("OpenCorsPolicy", policyBuilder =>
            {
                policyBuilder.SetIsOriginAllowed(x => true);
                policyBuilder.AllowAnyHeader();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowCredentials();
            });
        });
        builder.Services.AddSingleton<IDbInitializer, DbInitializer>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseRouting();

        app.UseCors("OpenCorsPolicy");

        app.UseHttpsRedirection();

        app.UseExceptionHandler();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await using (var scope = app.Services.CreateAsyncScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            try
            {
                await initializer.SeedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding admin users: {ex.Message}");
            }
        }

        app.Run();
    }
}