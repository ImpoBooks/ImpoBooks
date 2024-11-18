namespace ImpoBooks.DataAccess.Entities;

public interface IDbInitializer
{
    Task SeedAsync();
}