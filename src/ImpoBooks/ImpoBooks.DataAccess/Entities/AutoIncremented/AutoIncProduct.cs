using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	public class AutoIncProduct : BaseModelAutoIncExtended
	{
		[Column("book_id")] public int BookId { get; set; }
		[Reference(typeof(Book))] public Book Book { get; set; }
		[Column("coments")] public ICollection<Comment> Comments { get; set; }
	}
}
