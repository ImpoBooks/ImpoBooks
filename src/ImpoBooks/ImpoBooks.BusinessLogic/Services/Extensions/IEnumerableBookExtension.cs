using ImpoBooks.DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ImpoBooks.BusinessLogic.Services.Extensions
{
	public static class IEnumerableBookExtension
	{
		public static IEnumerable<Book> FilterByKeyWord(this IEnumerable<Book> source, string? keyWord)
		{
			if (keyWord.IsNullOrEmpty())
				return source;

			keyWord = keyWord!.ToLower();


			return source.Where(book =>
				keyWord!.Contains(book.Author.Person.Name.ToLower()) ||
				keyWord!.Contains(book.Author.Person.Surname.ToLower()) ||
				book.Name.ToLower().Contains(keyWord!) ||
				book.Genres!.Any(genre => genre.Name.ToLower().Contains(keyWord!)));
		}

		public static IEnumerable<Book> FilterByGenre(this IEnumerable<Book> source, string? genre)
		{
			if (genre.IsNullOrEmpty())
				return source;

			genre = genre!.ToLower();

			return source.Where(book => book.Genres!.Any(g => g.Name.ToLower().Contains(genre!)));
		}

		public static IEnumerable<Book> FilterByAuthor(this IEnumerable<Book> source, string? author)
		{

			if (author.IsNullOrEmpty())
				return source;

			author = author!.ToLower();

			return source.Where(book =>
				author!.Contains(book.Author.Person.Name.ToLower()) &&
				author!.Contains(book.Author.Person.Surname.ToLower()));
		}

		public static IEnumerable<Book> FilterByPrice(this IEnumerable<Book> source, decimal minPrice, decimal maxPrice)
		{
			return source.Where(book =>
				book.Price >= minPrice &&
				book.Price <= maxPrice);
		}

		public static IEnumerable<Book> FilterByRating(this IEnumerable<Book> source, decimal minRating, decimal maxRating)
		{
			return source.Where(book =>
				book.Rating >= minRating &&
				book.Rating <= maxRating);
		}
	}
}
