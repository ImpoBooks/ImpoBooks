using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class FilterModel
	{
		public string? KeyWords { get; set; }
		public string? Genre { get; set; }
		public string? Author { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
		public decimal MinRating { get; set; }
		public decimal MaxRating { get; set; }
	}
}
