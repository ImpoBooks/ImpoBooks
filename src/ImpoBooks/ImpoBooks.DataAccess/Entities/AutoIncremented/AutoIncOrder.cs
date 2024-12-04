using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("Orders")]
	public class AutoIncOrder : BaseModelAutoIncExtended
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
	}
}
