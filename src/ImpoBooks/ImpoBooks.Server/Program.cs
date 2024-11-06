using ImpoBooks.DataAccess;
using ImpoBooks.Infrastructure;
using ImpoBooks.Server.Middleware;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSupabaseClient(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExeptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers().AddNewtonsoftJson();
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