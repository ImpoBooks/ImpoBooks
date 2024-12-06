using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class BookSeeder
	{
		public static IEnumerable<Book> PreparedBooks =>
			new Book[]
			{
				new()
				{
					Id = 1,
					Name = "The Secret Garden",
					Description = "The story of a girl who discovers a magical garden and transforms her life",
					ReleaseDate = "2021.05.2",
					Rating = 4.8M,
					Format = "Electronic",
					Price = 25.99M,
					PublisherId = 3,
					AuthorId = 2,
					ImageUrl = ""
				},
				new()
				{
					Id = 2,
					Name = "Murder on the Orient Express",
					Description = "A classic detective story about an investigation aboard a train",
					ReleaseDate = "2019.08.25",
					Rating = 4.6M,
					Format = "Print",
					Price = 19.99M,
					PublisherId = 4,
					AuthorId = 1,
					ImageUrl = ""
				},
				new()
				{
					Id = 3,
					Name = "The Da Vinci Code",
					Description = "Puzzles, ancient symbols, and intrigue unfolding in the heart of the religious world",
					ReleaseDate = "2003.03.18",
					Rating = 4.3M,
					Format = "Electronic",
					Price = 38.99M,
					PublisherId = 2,
					AuthorId = 1,
					ImageUrl = ""
				},
				new()
				{
					Id = 4,
					Name = "The Picture of Dorian Gray",
					Description = "A moral tale about a young man obsessed with his beauty",
					ReleaseDate = "1998.07.12",
					Rating = 4.7M,
					Format = "Electronic",
					Price = 22.99M,
					PublisherId = 5,
					AuthorId = 5,
					ImageUrl = ""
				},
				new()
				{
					Id = 5,
					Name = "The Hobbit",
					Description = "The adventures of Bilbo Baggins in the fantastic world of Middle-earth",
					ReleaseDate = "1937.09.21",
					Rating = 4.9M,
					Format = "Print",
					Price = 27.99M,
					PublisherId = 2,
					AuthorId = 4,
					ImageUrl = ""
				}
			};

		public static string Seed =
			"INSERT INTO \"Books\" (id, name, description, release_date, format, publisher_id, author_id, price, image_url, rating) VALUES" +
			string.Join(", ", PreparedBooks
				.Select(b => $"(" +
				$"{b.Id}, " +
				$"'{b.Name}', " +
				$"'{b.Description}'," +
				$"'{b.ReleaseDate}'," +
				$"'{b.Format}', " +
				$"{b.PublisherId}, " +
				$"{b.AuthorId}, " +
				$"{b.Price.ToString().Replace(",", ".")}, " +
				$"'{b.ImageUrl}', " +
				$"{b.Rating.ToString().Replace(",", ".")})")) +
			";";
	}
}
