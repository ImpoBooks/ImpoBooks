using System.ComponentModel.DataAnnotations;

namespace ImpoBooks.Server.Request1s
{
	public class CatalogUpdateRequest
	{
		[Required] public int Id { get; set; }
		public string? Name { get; set; }
		public string? Genre { get; set; }
		public string? Author { get; set; }
		public string? Publisher { get; set; }
		public string? ReleaseDate { get; set; }
		public string? Description { get; set; }
		public string? Image { get; set; }
		public decimal Price { get; set; }
	}
}
