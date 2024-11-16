using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("Authors")]
	public class AutoIncAuthor : BaseModelAutoIncExtended
	{
		[Column("person_id")] public int PersonId { get; set; }
		[Reference(typeof(Person))] public Person Person { get; set; }
		[Column("books")] public ICollection<Book>? Books { get; set; }
	}
}
