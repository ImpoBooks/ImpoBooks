using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Fixtures
{
	public class CartSupabaseFixture : IAsyncLifetime
	{
		public Client client { get; private set; }

		public List<OrderModel> PreparedOrderModels =>
			new List<OrderModel>
			{
				new()
				{
					FirstName = "John",
					LastName = "Doe",
					Email = "john.doe@example.com",
					Address = "123 Elm Street",
					City = "Springfield",
					ZipCode = "12345",
					Country = "USA",
					TotalSum = 100.97m,
					Products = new OrderProductModel[]
					{
						new() {Id =  1, Count = 2},
						new() {Id =  3, Count = 1},
					}
				},
				new()
				{
					FirstName = "Jane",
					LastName = "Smith",
					Email = "jane.smith@example.com",
					Address = "456 Oak Avenue",
					City = "Metropolis",
					ZipCode = "54321",
					Country = "Canada",
					TotalSum = 162.95m,
					Products = new OrderProductModel[]
					{
						new() {Id =  1, Count = 1},
						new() {Id =  2, Count = 1},
						new() {Id =  3, Count = 3},
					}
				},
				new()
				{
					FirstName = "Alice",
					LastName = "Johnson",
					Email = "alice.johnson@example.com",
					Address = "789 Pine Road",
					City = "Gotham",
					ZipCode = "67890",
					Country = "UK",
					TotalSum = 199.90m,
					Products = new OrderProductModel[]
					{
						new() {Id =  2, Count = 10},
					}
				},
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
				new() { Id = 3, PersonId = 2},
			};


		public IEnumerable<Person> PreparedPersons =>
			new Person[]
			{
				new() { Id = 4, Name = "Volodymyr", Surname = "Tkachenko"},
				new() { Id = 3, Name = "Andriy", Surname = "Grytsenko"},
				new() { Id = 2, Name = "Dmytro", Surname = "Kovalchuk"},
			};

		public IEnumerable<Publisher> PreparedPublishers =>
			new Publisher[]
			{
				new() { Id = 2, Name = "Smoloskyp"},
				new() { Id = 3, Name = "Old Lion Publishing House"},
				new() { Id = 4, Name = "Nash Format"},
			};

		public IEnumerable<Genre> PreparedGenres =>
			new Genre[]
			{
				new() { Id = 1, Name = "Science-Fiction"},
				new() { Id = 2, Name = "Detective"},
				new() { Id = 4, Name = "Adventure"},
			};

		public IEnumerable<BookGenre> PreparedBookGenreRelations =>
			new BookGenre[]
			{
				new() { Id = 1, BookId = 1, GenreId = 2 },
				new() { Id = 2, BookId = 1, GenreId = 4 },
				new() { Id = 3, BookId = 2, GenreId = 1 },
				new() { Id = 4, BookId = 3, GenreId = 4 },
				new() { Id = 5, BookId = 3, GenreId = 1 },

			};

		public async Task DisposeAsync(){
			await IntegrationTestHelper.ClearTable<Order>(client);
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
			await IntegrationTestHelper.InitTable(client, PreparedPersons);
			await IntegrationTestHelper.InitTable(client, PreparedAuthors);
			await IntegrationTestHelper.InitTable(client, PreparedGenres);
			await IntegrationTestHelper.InitTable(client, PreparedPublishers);
			await IntegrationTestHelper.InitTable(client, PreparedBooks);
			await IntegrationTestHelper.InitTable(client, PreparedBookGenreRelations);
		}
	}
}
