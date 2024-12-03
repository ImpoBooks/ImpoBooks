using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class CustomerRequisitesModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string CardNumber { get; set; }
		public string ExpirationDate { get; set; }
		public string CVV { get; set; }
		public decimal TotalSum { get; set; }
	}
}
