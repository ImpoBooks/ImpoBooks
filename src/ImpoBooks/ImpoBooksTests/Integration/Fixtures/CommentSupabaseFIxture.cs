using ImpoBooks.DataAccess.Entities;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Fixtures
{
	public class CommentSupabaseFIxture : IAsyncLifetime
	{
		public Client client { get; private set; }

		public IEnumerable<Comment> PreparedComments =>
			new Comment[]
			{
				new() 
				{
					Id = 1,
					UserId = 1,
					ProductId = 1,
					Content = "Cool book",
					LikesNumber = 7,
					DislikesNumber = 1,
					Rating = 4.5M
				},
				new() {
					Id = 2,
					UserId = 2,
					ProductId = 2,
					Content = "I love this book", 
					LikesNumber = 9, 
					DislikesNumber = 0,
					Rating = 4.7M
				},
				new() 
				{
					Id = 3,
					UserId = 2,
					ProductId = 3,
					Content = "All recommend this one",
					LikesNumber = 8,
					DislikesNumber = 0,
					Rating = 4.3M
				},
				new()
				{
					Id = 4,
					UserId = 3,
					ProductId = 4,
					Content = "When will there be a sequel?",
					LikesNumber = 11,
					DislikesNumber = 2,
					Rating = 4.5M
				},
				new() 
				{
					Id = 5,
					UserId = 4,
					ProductId = 5,
					Content = "I don`t like it",
					LikesNumber = 1,
					DislikesNumber = 5,
					Rating = 3.3M
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
				}
			};
		public IEnumerable<Publisher> PreparedPublishers =>
			new Publisher[]
			{
				new() { Id = 3, Name = "Old Lion Publishing House"}
			};

		public IEnumerable<Author> PreparedAuthors =>
			new Author[]
			{
				new() { Id = 2, PersonId = 3},
			};

		public IEnumerable<Person> PreparedPersons =>
			new Person[]
			{
				new() { Id = 3, Name = "Andriy", Surname = "Grytsenko"},
			};

		public IEnumerable<Product> PreparedProducts =>
			new Product[]
			{
				new() {Id = 1, BookId = 1},
				new() {Id = 2, BookId = 1},
				new() {Id = 3, BookId = 1},
				new() {Id = 4, BookId = 1},
				new() {Id = 5, BookId = 1},
			};

		public IEnumerable<User> PreparedUsers =>
			new User[]
			{
				new() {Id = 1, Name = "Mikhail"},
				new() {Id = 2, Name = "Bender"},
				new() {Id = 3, Name = "Kyle"},
				new() {Id = 4, Name = "Nikita"},
			};
		public async Task DisposeAsync()
		{
			await IntegrationTestHelper.ClearTable<Publisher>(client);
			await IntegrationTestHelper.ClearTable<Person>(client);
			await IntegrationTestHelper.ClearTable<Author>(client);
			await IntegrationTestHelper.ClearTable<Book>(client);
			await IntegrationTestHelper.ClearTable<User>(client);
			await IntegrationTestHelper.ClearTable<Comment>(client);
			await IntegrationTestHelper.ClearTable<Product>(client);
		}

		public async Task InitializeAsync()
		{
			client = IntegrationTestHelper.TestClientInit();
			await IntegrationTestHelper.InitTable(client, PreparedUsers);
			await IntegrationTestHelper.InitTable(client, PreparedPersons);
			await IntegrationTestHelper.InitTable(client, PreparedAuthors);
			await IntegrationTestHelper.InitTable(client, PreparedPublishers);
			await IntegrationTestHelper.InitTable(client, PreparedBooks);
			await IntegrationTestHelper.InitTable(client, PreparedProducts);
			await IntegrationTestHelper.InitTable(client, PreparedComments);
		}
	}
}
