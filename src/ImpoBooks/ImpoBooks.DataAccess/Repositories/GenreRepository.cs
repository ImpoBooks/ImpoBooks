using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using Supabase;
using Supabase.Postgrest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Supabase.Postgrest.Constants;

namespace ImpoBooks.DataAccess.Repositories
{
	public class GenreRepository(Client client) : Repository<Genre>(client), IGenreRepository
	{
		private readonly Client _client = client;

		public override async Task<Genre> GetByIdAsync(int id)
		{
			ModeledResponse<Genre> response = await _client.From<Genre>().Where(x => x.Id == id).Get();
			Genre genre = response.Model;
			if (genre is null) return genre;

			await SetBooks(genre);

			return genre;
		}

		public override async Task<IEnumerable<Genre>> GetAllAsync()
		{
			ModeledResponse<Genre> response = await _client.From<Genre>().Get();
			IEnumerable<Genre> genres = response.Models;

			foreach (Genre genre in genres) await SetBooks(genre);

			return genres;
		}

		public async Task<Genre> GetByNameAsync(string name)
		{
			ModeledResponse<Genre> response = await _client.From<Genre>().Where(x => x.Name == name).Get();
			Genre genre = response.Model;
			if (genre is null) return genre;

			await SetBooks(genre);

			return genre;
		}

		private async Task SetBooks(Genre genre)
		{
			ModeledResponse<BookGenre> responseBG = await _client.From<BookGenre>()
				.Select("*, Genres(*))")
				.Filter("genre_id", Operator.Equals, genre.Id)
				.Get();

			IEnumerable<BookGenre> relation = responseBG.Models;
			ICollection<Book> books = relation.Select(x => x.Book).ToList();
			genre.Books = books;
		}
	}
}
