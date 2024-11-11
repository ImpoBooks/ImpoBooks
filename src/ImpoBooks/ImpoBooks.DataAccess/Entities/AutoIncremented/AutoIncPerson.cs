using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("Persons")]
	public class AutoIncPerson : BaseModelAutoIncExtended
	{
		[Column("name")] public string Name { get; set; }
		[Column("surname")] public string Surname { get; set; }
	}
}
