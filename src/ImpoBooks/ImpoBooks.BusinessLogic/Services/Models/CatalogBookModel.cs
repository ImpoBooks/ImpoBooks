using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class CatalogBookModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public string Genres { get; set; }
		public string ReleaseDate { get; set; }
		public decimal Rating { get; set; }
		public decimal Price { get; set; }
		public string ImageUrl { get; set; }

		public override bool Equals(object obj)
		{
			return obj is CatalogBookModel book &&
			Id == book.Id &&
			Name == book.Name &&
			Author == book.Author &&
			Genres == book.Genres &&
			ReleaseDate == book.ReleaseDate &&
			Price == book.Price &&
			Rating == book.Rating;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}
	