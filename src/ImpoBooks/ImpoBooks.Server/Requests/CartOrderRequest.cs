using System.ComponentModel.DataAnnotations;

namespace ImpoBooks.Server.Requests
{
	public class CartOrderRequest
	{
		[Required] public string FirstName { get; set; }
		[Required] public string LastName { get; set; }
		[Required] public string Email { get; set; }
		[Required] public string Address { get; set; }
		[Required] public string City { get; set; }
		[Required] public string ZipCode { get; set; }
		[Required] public string Country { get; set; }
		[Required] public string CardNumber { get; set; }
		[Required] public string ExpirationDate { get; set; }
		[Required] public string CVV { get; set; }
		[Required] public decimal TotalSum { get; set; }

		[Required] public IEnumerable<ProductInfo> Products { get; set; }
	}

	public record ProductInfo(int Id, int Count);
}
