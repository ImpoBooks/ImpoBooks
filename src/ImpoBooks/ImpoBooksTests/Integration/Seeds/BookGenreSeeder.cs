using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class BookGenreSeeder
	{
		public static IEnumerable<BookGenre> PreparedBookGenreRelations =>
			new BookGenre[]
			{
				new() { Id = 1, BookId = 1, GenreId = 2 },
				new() { Id = 2, BookId = 1, GenreId = 4 },
				new() { Id = 3, BookId = 2, GenreId = 1 },
				new() { Id = 4, BookId = 3, GenreId = 4 },
				new() { Id = 5, BookId = 3, GenreId = 1 },
				new() { Id = 6, BookId = 5, GenreId = 1 },
				new() { Id = 7, BookId = 4, GenreId = 2 },
			};

		public static string Seed =
			"INSERT INTO \"BooksGenres\" (id, book_id, genre_id) VALUES" +
			string.Join(", ", PreparedBookGenreRelations.Select(bg => $"({bg.Id}, {bg.BookId}, {bg.GenreId})")) +
			";";
	}
}
