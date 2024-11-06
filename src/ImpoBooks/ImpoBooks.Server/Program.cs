using ImpoBooks.BusinessLogic.Services;
using ImpoBooks.DataAccess;
using ImpoBooks.DataAccess.Repositories.Users;
using ImpoBooks.Infrastructure;
using ImpoBooks.Infrastructure.Providers;
using ImpoBooks.Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IUsersRepository, UsersRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddSupabaseClient(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExeptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddRouting(options => { options.LowercaseUrls = true; });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();