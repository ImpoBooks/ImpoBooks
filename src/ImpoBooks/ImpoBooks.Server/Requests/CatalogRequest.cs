namespace ImpoBooks.Server.Requests
{
	public class CatalogRequest
	{
		public string? KeyWords { get; set; }
		public string? Genre { get; set; }
		public string? Author { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
		public decimal MinRating { get; set; }
		public decimal MaxRating { get; set; }
		public int TotalItems { get; set; } = 30;
	}
}
