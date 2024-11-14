using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.Server.Requests;

namespace ImpoBooks.Server.Extensions
{
	public static class CatalogExtensions
	{
		public static FilterModel ToModel(this CatalogRequest source)
		{
			return new FilterModel() 
			{ 
				KeyWords = source.KeyWords,
				Genre = source.Genre,
				Author	= source.Author,
				MinPrice = source.MinPrice,
				MaxPrice = source.MaxPrice,
				MinRating = source.MinRating,
				MaxRating = source.MaxRating,
			};
		}
	}
}
