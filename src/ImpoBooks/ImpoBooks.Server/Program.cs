using ImpoBooks.BusinessLogic.Extensions;
using ImpoBooks.BusinessLogic.Services.Auth;
using ImpoBooks.BusinessLogic.Services.Catalog;
using ImpoBooks.BusinessLogic.Services.Product;
using ImpoBooks.DataAccess;
using ImpoBooks.Infrastructure;
using ImpoBooks.Infrastructure.Providers;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepository();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<ICatalogService, CatalogService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddSupabaseClient(builder.Configuration);
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExeptionHandler>();
builder.Services.AddProblemDetails();
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

app.Run();