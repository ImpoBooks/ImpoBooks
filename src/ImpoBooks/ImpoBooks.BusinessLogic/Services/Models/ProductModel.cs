using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class ProductModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Format { get; set; }
		public string Author { get; set; }
		public string Publisher { get; set; }
		public string Genres { get; set; }
		public string ReleaseDate { get; set; }
		public decimal Rating { get; set; }
		public decimal Price { get; set; }
		public IEnumerable<CommentModel> Comments { get; set; }

		public override bool Equals(object obj)
		{
			return obj is ProductModel product &&
			Id == product.Id &&
			Name == product.Name &&
			Description == product.Description &&
			Format == product.Format &&
			Author == product.Author &&
			Publisher == product.Publisher &&
			Genres == product.Genres &&
			ReleaseDate == product.ReleaseDate &&
			Rating == product.Rating &&
			Price == product.Price &&
			Comments?.Count() == product.Comments?.Count();
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}
