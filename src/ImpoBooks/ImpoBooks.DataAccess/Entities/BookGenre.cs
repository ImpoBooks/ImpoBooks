using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("BooksGenres")]
	public class BookGenre : BaseModelExtended
	{
		[Column("book_id")] public int BookId { get; set; }
		[Column("genre_id")] public int GenreId { get; set; }

		[Reference(typeof(Book))] public Book Book { get; set; }
		[Reference(typeof(Genre))] public Genre Genre { get; set; }
	}
}
