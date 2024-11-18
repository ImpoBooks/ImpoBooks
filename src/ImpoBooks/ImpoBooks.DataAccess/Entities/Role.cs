using Supabase.Postgrest.Attributes;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented;

[Table("Roles")]
public class Role : BaseModelExtended
{
    [PrimaryKey("id", false)] public int Id { get; set; }
    [Column("name")] public string Name { get; set; }
}