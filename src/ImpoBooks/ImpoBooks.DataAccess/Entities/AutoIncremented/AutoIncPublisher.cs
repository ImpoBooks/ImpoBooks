using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("Publishers")]
	public class AutoIncPublisher : BaseModelAutoIncExtended
	{
		[Column("name")] public string Name { get; set; }
		[Column("books")] public ICollection<Book>? Books { get; set; }
	}
}
