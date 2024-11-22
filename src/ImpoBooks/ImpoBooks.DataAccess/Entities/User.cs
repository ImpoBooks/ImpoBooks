using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Interfaces;
using Supabase.Postgrest.Attributes;

namespace ImpoBooks.DataAccess.Entities;

[Table("Users")]
public class User : BaseModelExtended, IAutoInc<AutoIncUser>
{
    [Column("name")] public string Name { get; set; }
    [Column("email")] public string Email { get; set; }
    [Column("hashed_password")] public string HashedPassword { get; set; }
    [Column("role_id")] public int RoleId { get; set; } = 2;
    [Reference(typeof(Role))] public Role Role { get; set; }

	public override bool Equals(object obj)
	{
		return obj is User user &&
				Id == user.Id &&
				Email == user.Email &&
				HashedPassword == user.HashedPassword;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Id);
	}

	public AutoIncUser ToAutoInc()
	{
		return new AutoIncUser()
		{
			Id = Id,
			Name = Name,
			Email = Email,
			HashedPassword = HashedPassword
		};
	}
}