using System.ComponentModel.DataAnnotations;

namespace ImpoBooks.Server.Requests
{
	public class CatalogCreateRequest
	{
		[Required] public string Name { get; set; }
		[Required] public string Genre { get; set; }
		[Required] public string Author { get; set; }
		[Required] public string Publisher { get; set; }
		[Required] public string ReleaseDate { get; set; }
		[Required] public string Description { get; set; }
		[Required] public string ImageUrl { get; set; }
		[Required] public decimal Price { get; set; }
	}
}
