using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class OrderCheckModel
	{
		public int OrderCode { get; set; }
		public string CreatedAt { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
		public decimal TotalSum { get; set; }
		public string ExpensesList { get; set; }

		public override bool Equals(object obj)
		{
			return obj is OrderCheckModel order &&
			OrderCode == order.OrderCode &&
			FirstName == order.FirstName &&
			LastName == order.LastName &&
			Email == order.Email &&
			Address == order.Address &&
			City == order.City &&
			ZipCode == order.ZipCode &&
			Country == order.Country &&
			TotalSum == order.TotalSum &&
			ExpensesList == order.ExpensesList;
		}
	}
}
