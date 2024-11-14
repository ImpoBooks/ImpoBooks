using ImpoBooks.DataAccess.Entities;
using ImpoBooksTests;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.DataTests.Fixtures
{
	public class BookSupabaseFixture : IAsyncLifetime
	{
		public Client client { get; private set; }
		public IEnumerable<Book> PrepearedBooks =>
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
				}
			};
		public IEnumerable<Author> PrepearedAuthors =>
			new Author[]
			{
				new() { Id = 1, PersonId = 4},
				new() { Id = 2, PersonId = 3},
				new() { Id = 3, PersonId = 2},
				new() { Id = 4, PersonId = 6},
				new() { Id = 5, PersonId = 1}
			};


		public IEnumerable<Person> PrepearedPersons =>
			new Person[]
			{
				new() { Id = 4, Name = "Volodymyr", Surname = "Tkachenko"},
				new() { Id = 3, Name = "Andriy", Surname = "Grytsenko"},
				new() { Id = 2, Name = "Dmytro", Surname = "Kovalchuk"},
				new() { Id = 6, Name = "Olha", Surname = "Syrenko"},
				new() { Id = 1, Name = "Oleksandr", Surname = "Shevchenko"}
			};

		public IEnumerable<Publisher> PrepearedPublishers =>
			new Publisher[]
			{
				new() { Id = 1, Name = "Ranok"},
				new() { Id = 2, Name = "Smoloskyp"},
				new() { Id = 3, Name = "Old Lion Publishing House"},
				new() { Id = 4, Name = "Nash Format"},
				new() { Id = 5, Name = "Vivat"}
			};

		public IEnumerable<Genre> PrepearedGenres =>
			new Genre[]
			{
				new() { Id = 1, Name = "Science Fiction"},
				new() { Id = 2, Name = "Detective"},
				new() { Id = 3, Name = "Detective"},
				new() { Id = 4, Name = "Adventure"},
				new() { Id = 5, Name = "Fantasy"}
			};

		public IEnumerable<BookGenre> PrepearedBookGenreRelations =>
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

		public async Task DisposeAsync()
		{
			await IntegrationTestHelper.ClearTable<Genre>(client);
			await IntegrationTestHelper.ClearTable<Publisher>(client);
			await IntegrationTestHelper.ClearTable<Person>(client);
			await IntegrationTestHelper.ClearTable<Author>(client);
			await IntegrationTestHelper.ClearTable<Book>(client);
			await IntegrationTestHelper.ClearTable<BookGenre>(client);
		}

		public async Task InitializeAsync()
		{
			client = IntegrationTestHelper.TestClientInit();
			await IntegrationTestHelper.InitTable(client, PrepearedPersons);
			await IntegrationTestHelper.InitTable(client, PrepearedAuthors);
			await IntegrationTestHelper.InitTable(client, PrepearedGenres);
			await IntegrationTestHelper.InitTable(client, PrepearedPublishers);
			await IntegrationTestHelper.InitTable(client, PrepearedBooks);
			await IntegrationTestHelper.InitTable(client, PrepearedBookGenreRelations);
		}

	}
}
