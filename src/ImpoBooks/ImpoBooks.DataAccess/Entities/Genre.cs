using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Interfaces;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Genres")]
	public class Genre : BaseModelExtended, IAutoInc<AutoIncGenre>
	{
		[Column("name")] public string Name { get; set; }
		[Column("books")] public ICollection<Book>? Books { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Genre genre &&
			Id == genre.Id &&
			Name == genre.Name &&
			Books?.Count == genre.Books?.Count;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		public AutoIncGenre ToAutoInc()
		{
			return new AutoIncGenre()
			{
				Id = Id,
				Name = Name,
				Books = Books
			};
		}
	}
}
