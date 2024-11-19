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
					Content = "Cool book",
					LikesNumber = 7,
					DislikesNumber = 1,
					Rating = 4.5M
				},
				new() {
					Id = 2,
					UserId = 2,
					Content = "I love this book", 
					LikesNumber = 9, 
					DislikesNumber = 0,
					Rating = 4.7M
				},
				new() 
				{
					Id = 3,
					UserId = 2,
					Content = "All recommend this one",
					LikesNumber = 8,
					DislikesNumber = 0,
					Rating = 4.3M
				},
				new()
				{
					Id = 4,
					UserId = 3,
					Content = "When will there be a sequel?",
					LikesNumber = 11,
					DislikesNumber = 2,
					Rating = 4.5M
				},
				new() 
				{
					Id = 5,
					UserId = 4,
					Content = "I don`t like it",
					LikesNumber = 1,
					DislikesNumber = 5,
					Rating = 3.3M
				},

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
			await IntegrationTestHelper.ClearTable<Comment>(client);
			await IntegrationTestHelper.ClearTable<User>(client);
		}

		public async Task InitializeAsync()
		{
			client = IntegrationTestHelper.TestClientInit();
			await IntegrationTestHelper.InitTable(client, PreparedUsers);
			await IntegrationTestHelper.InitTable(client, PreparedComments);
		}
	}
}
