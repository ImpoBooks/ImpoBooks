using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Orders")]
	public class Order : BaseModelExtended, IAutoInc<AutoIncOrder>
	{
		[Column("order_code")] public int OrderCode { get; set; }
		[Column("created_at")] public string CreatedAt { get; set; }
		[Column("first_name")] public string FirstName { get; set; }
		[Column("last_name")] public string LastName { get; set; }
		[Column("email")] public string Email { get; set; }
		[Column("address")] public string Address { get; set; }
		[Column("city")] public string City { get; set; }
		[Column("zip_code")] public string ZipCode { get; set; }
		[Column("country")] public string Country { get; set; }
		[Column("total_sum")] public decimal TotalSum { get; set; }
		[Column("products")] public string ExpensesList { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Order order &&
			Id == order.Id &&
			OrderCode == order.OrderCode &&
			CreatedAt == order.CreatedAt &&
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

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		public AutoIncOrder ToAutoInc()
		{
			return new AutoIncOrder()
			{
				Id = Id,
				OrderCode = OrderCode,
				CreatedAt = CreatedAt,
				FirstName = FirstName,
				LastName = LastName,
				Email = Email,
				Address = Address,
				City = City,
				ZipCode = ZipCode,
				Country = Country,
				TotalSum = TotalSum,
				ExpensesList = ExpensesList
			};
		}
	}
}
