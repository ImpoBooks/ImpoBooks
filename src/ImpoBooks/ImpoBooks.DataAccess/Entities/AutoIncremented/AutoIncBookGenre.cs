using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("BooksGenres")]
	public class AutoIncBookGenre : BaseModelAutoIncExtended
	{
		[Column("book_id")] public int BookId { get; set; }
		[Column("genre_id")] public int GenreId { get; set; }

		[Reference(typeof(Book))] public Book Book { get; set; }
		[Reference(typeof(Genre))] public Genre Genre { get; set; }
	}
}
