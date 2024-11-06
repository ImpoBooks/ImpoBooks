using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ImpoBooks.DataAccess.Entities;

public class User : BaseModel
{
    [PrimaryKey("id", false)] public int Id { get; set; }
    [Column("name")] public string Name { get; set; }
    [Column("email")] public string Email { get; set; }
    [Column("hashed_password")] public string HashedPassword { get; set; }
}