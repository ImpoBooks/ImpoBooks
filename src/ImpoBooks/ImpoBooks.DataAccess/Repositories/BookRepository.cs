using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using Supabase;
using Supabase.Postgrest.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Supabase.Postgrest.Constants;

namespace ImpoBooks.DataAccess.Repositories
{
	public class BookRepository(Client client) : Repository<Book>(client), IBookRepository
	{
		private readonly Client _client = client;

		public override async Task<Book> GetByIdAsync(int id)
		{
			ModeledResponse<Book> response = await _client.From<Book>().Where(x => x.Id == id).Get();
			Book book = response.Model;
			if (book is null) return book;

			await SetGenres(book);

			return book;
		}
		public override async Task<IEnumerable<Book>> GetAllAsync()
		{
			ModeledResponse<Book> response = await _client.From<Book>().Get();
			IEnumerable<Book> books = response.Models;

			foreach (Book book in books) await SetGenres(book);

			return books;
		}

		public async Task<IEnumerable<Book>> GetByAuthorFullNameAsync(string name, string surname)
		{
			ModeledResponse<Book> response = await _client.From<Book>()
				.Select("*, Publishers(*),Authors!inner(*, Persons!inner(*)))")
				.Filter("Authors.Persons.name", Operator.Equals, name)
				.Filter("Authors.Persons.surname", Operator.Equals, surname)
				.Get();

			IEnumerable<Book> books = response.Models;

			foreach (Book book in books) await SetGenres(book);

			return books;
		}

		public async Task<IEnumerable<Book>> GetByFormatAsync(string format)
		{
			ModeledResponse<Book> response = await _client.From<Book>().Where(x => x.Format == format).Get();
			IEnumerable<Book> books = response.Models;

			foreach (Book book in books) await SetGenres(book);

			return books;
		}

		public async Task<Book> GetByNameAsync(string name)
		{
			ModeledResponse<Book> response = await _client.From<Book>().Where(x => x.Name == name).Get();
			Book book = response.Model;
			if (book is null) return book;

			await SetGenres(book);

			return book;
		}

		public async Task<IEnumerable<Book>> GetByPriceAsync(decimal price)
		{
			string formattedPrice = price.ToString(System.Globalization.CultureInfo.InvariantCulture);

			ModeledResponse<Book> response = await _client.From<Book>()
				.Select("*")
				.Filter("price", Operator.Equals, formattedPrice).Get();

			IEnumerable<Book> books = response.Models;

			foreach (Book book in books) await SetGenres(book);

			return books;
		}

		public async Task<IEnumerable<Book>> GetByPublisherNameAsync(string name)
		{
			ModeledResponse<Book> response = await _client.From<Book>()
				.Select("*, Authors(*, Persons(*)), Publishers!inner(*))")
				.Filter("Publishers.name", Operator.Equals, name)
				.Get();

			IEnumerable<Book> books = response.Models;

			foreach (Book book in books) await SetGenres(book);

			return books;
		}

		public async Task<IEnumerable<Book>> GetByRaitingAsync(decimal raiting)
		{
			string formattedRating = raiting.ToString(System.Globalization.CultureInfo.InvariantCulture);

			ModeledResponse<Book> response = await _client.From<Book>()
				.Select("*")
				.Filter("rating", Operator.Equals, formattedRating).Get();

			IEnumerable<Book> books = response.Models;

			foreach (Book book in books) await SetGenres(book);

			return books;
		}

		public async Task<IEnumerable<Book>> GetByReleasDateAsync(DateTime releasDate)
		{
			ModeledResponse<Book> response = await _client.From<Book>().Where(x => x.ReleaseDate == releasDate).Get();
			IEnumerable<Book> books = response.Models;

			foreach (Book book in books) await SetGenres(book);

			return books;
		}

		private async Task SetGenres(Book book)
		{
			ModeledResponse<BookGenre> responseBG = await _client.From<BookGenre>()
				.Select("*, Genres(*))")
				.Filter("book_id", Operator.Equals, book.Id)
				.Get();

			IEnumerable<BookGenre> relation = responseBG.Models;
			ICollection<Genre> genres = relation.Select(x => x.Genre).ToList();
			book.Genres = genres;
		}
	}
}
