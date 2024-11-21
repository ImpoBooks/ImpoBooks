using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class ProductCommentModel
	{
		public int UserId { get; set; }
		public string Content { get; set; }
		public decimal Rating { get; set; }
	}
}
