using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class BookModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public string Genres { get; set; }
		public string Publisher { get; set; }
		public string ReleaseDate { get; set; }
		public string ImageUrl { get; set; }
		public decimal Rating { get; set; }
		public decimal Price { get; set; }
	}
}
