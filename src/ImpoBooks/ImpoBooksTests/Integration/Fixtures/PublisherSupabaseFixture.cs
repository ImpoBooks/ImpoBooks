using ImpoBooks.DataAccess.Entities;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Fixtures
{
	public class PublisherSupabaseFixture : IAsyncLifetime
	{
		public Client client { get; private set; }
		public IEnumerable<Publisher> PreparedPublishers =>
			new Publisher[]
			{
				new() { Id = 1, Name = "Ranok"},
				new() { Id = 2, Name = "Smoloskyp"},
				new() { Id = 3, Name = "Old Lion Publishing House"},
				new() { Id = 4, Name = "Nash Format"},
				new() { Id = 5, Name = "Vivat"}
			};

		public IEnumerable<Book> PreparedBooks =>
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
				}
			};

		public IEnumerable<Author> PreparedAuthors =>
			new Author[]
			{
				new() { Id = 1, PersonId = 4},
				new() { Id = 2, PersonId = 3},
			};


		public IEnumerable<Person> PreparedPersons =>
			new Person[]
			{
				new() { Id = 4, Name = "Volodymyr", Surname = "Tkachenko"},
				new() { Id = 3, Name = "Andriy", Surname = "Grytsenko"},
			};

		public IEnumerable<Genre> PreparedGenres =>
			new Genre[]
			{
				new() { Id = 1, Name = "Science Fiction"},
				new() { Id = 2, Name = "Detective"},
				new() { Id = 3, Name = "Detective"},
				new() { Id = 4, Name = "Adventure"},
				new() { Id = 5, Name = "Fantasy"}
			};

		public async Task DisposeAsync()
		{
			await IntegrationTestHelper.ClearTable<Person>(client);
			await IntegrationTestHelper.ClearTable<Author>(client);
			await IntegrationTestHelper.ClearTable<Genre>(client);
			await IntegrationTestHelper.ClearTable<Publisher>(client);
			await IntegrationTestHelper.ClearTable<Book>(client);
		}

		public async Task InitializeAsync()
		{
			client = IntegrationTestHelper.TestClientInit();
			await IntegrationTestHelper.InitTable(client, PreparedPublishers);
			await IntegrationTestHelper.InitTable(client, PreparedPersons);
			await IntegrationTestHelper.InitTable(client, PreparedAuthors);
			await IntegrationTestHelper.InitTable(client, PreparedGenres);
			await IntegrationTestHelper.InitTable(client, PreparedBooks);
		}

	}
}
