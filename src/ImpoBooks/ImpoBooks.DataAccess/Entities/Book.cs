using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Interfaces;
using Newtonsoft.Json;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Books")]
	public class Book : BaseModelExtended, IAutoInc<AutoIncBook>
	{
		[Column("name")] public string Name { get; set; }
		[Column("publisher_id")] public int PublisherId { get; set; }
		[Column("author_id")] public int AuthorId { get; set; }
		[Column("description")] public string Description { get; set; }
		[Column("release_date")] public string ReleaseDate { get; set; }
		[Column("price")] public decimal Price { get; set; }
		[Column("rating")] public decimal Rating { get; set; }
		[Column("format")] public string Format { get; set; }
		[Column("image_url")] public string ImageUrl { get; set; }
		[Reference(typeof(Publisher))] public Publisher Publisher { get; set; }
		[Reference(typeof(Author))] public Author Author { get; set; }
		[Column("genres")] public ICollection<Genre>? Genres { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Book book &&
			Id == book.Id &&
			Name == book.Name &&
			PublisherId == book.PublisherId &&
			AuthorId == book.AuthorId &&
			Description == book.Description &&
			ReleaseDate == book.ReleaseDate &&
			Price == book.Price &&
			Rating == book.Rating &&
			Format == book.Format &&
			Publisher.Equals(book.Publisher) &&
			Author.Equals(book.Author) &&
			Genres?.Count == book.Genres?.Count;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		public AutoIncBook ToAutoInc()
		{
			return new AutoIncBook()
			{
				Id = Id,
				Name = Name,
				PublisherId = PublisherId,
				AuthorId = AuthorId,
				Description = Description,
				ReleaseDate = ReleaseDate,
				Price = Price,
				Rating = Rating,
				Format = Format,
				Publisher = Publisher,
				Author = Author,
				Genres = Genres,
				ImageUrl = ImageUrl
			};
		}
	}
}
