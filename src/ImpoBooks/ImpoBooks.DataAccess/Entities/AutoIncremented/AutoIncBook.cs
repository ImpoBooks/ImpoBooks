using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("Books")]
	public class AutoIncBook : BaseModelAutoIncExtended
	{
		[Column("name")] public string Name { get; set; }
		[Column("publisher_id")] public int PublisherId { get; set; }
		[Column("author_id")] public int AuthorId { get; set; }
		[Column("description")] public string Description { get; set; }
		[Column("release_date")] public string ReleaseDate { get; set; }
		[Column("price")] public decimal Price { get; set; }
		[Column("rating")] public decimal Rating { get; set; }
		[Column("format")] public string Format { get; set; }
		[Reference(typeof(Publisher))] public Publisher Publisher { get; set; }
		[Reference(typeof(Author))] public Author Author { get; set; }
		[Column("genres")] public ICollection<Genre>? Genres { get; set; }
	}
}
