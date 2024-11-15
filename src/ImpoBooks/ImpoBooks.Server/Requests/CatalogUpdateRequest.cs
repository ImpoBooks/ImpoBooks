namespace ImpoBooks.Server.Requests
{
	public class CatalogUpdateRequest
	{
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
