using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.Server.Request1s;
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

		public static BookModel ToModel(this CatalogCreateRequest source)
		{
			return new BookModel()
			{
				Name = source.Name,
				Description = source.Description,
				Author = source.Author,
				Genres = source.Genre,
				Publisher = source.Publisher,
				ReleaseDate = source.ReleaseDate,
				Image = source.Image,
				Rating = 0,
				Price = source.Price,
			};
		}

		public static BookModel ToModel(this CatalogUpdateRequest source)
		{
			return new BookModel()
			{
				Name = source.Name,
				Description = source.Description,
				Author = source.Author,
				Genres = source.Genre,
				Publisher = source.Publisher,
				ReleaseDate = source.ReleaseDate,
				Image = source.Image,
				Rating = 0,
				Price = source.Price,
			};
		}
	}
}
