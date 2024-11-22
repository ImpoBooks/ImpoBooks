using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("Users")]
	public class AutoIncUser : BaseModelAutoIncExtended
	{
		[Column("name")] public string Name { get; set; }
		[Column("email")] public string Email { get; set; }
		[Column("hashed_password")] public string HashedPassword { get; set; }
		[Column("role_id")] public int RoleId { get; set; }
		[Reference(typeof(Role))] public Role Role { get; set; }
	}
}
