using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using Microsoft.Extensions.Configuration;
using Supabase;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Integration Tests Collection")]
	public class CommentRepositoryTests : IClassFixture<CommentSupabaseFIxture>
	{
		private readonly CommentSupabaseFIxture _fixture;
		private readonly Client _client;
		private readonly CommentRepository _repository;
		private IEnumerable<Comment> _preparedComments;
		private IEnumerable<User> _preparedUsers;
		private IEnumerable<Author> _preparedAuthors;
		private IEnumerable<Publisher> _preparedPublishers;
		private IEnumerable<Book> _preparedBooks;

		public CommentRepositoryTests(CommentSupabaseFIxture fixture)
		{
			_fixture = fixture;
			_client = fixture.client;
			_repository = new(fixture.client);
			_preparedAuthors = fixture.PreparedAuthors
				.Select(x => new Author()
				{
					Id = x.Id,
					PersonId = x.PersonId,
					Person = fixture.PreparedPersons.First(p => p.Id == x.PersonId)
				});
			_preparedPublishers = fixture.PreparedPublishers;
			_preparedBooks = fixture.PreparedBooks
				.Select(x => new Book()
				{
					Id = x.Id,
					PublisherId = x.PublisherId,
					AuthorId = x.AuthorId,
					Name = x.Name,
					Description = x.Description,
					ReleaseDate = x.ReleaseDate,
					Price = x.Price,
					Rating = x.Rating,
					Format = x.Format,
					Publisher = _preparedPublishers.First(p => p.Id == x.PublisherId),
					Author = _preparedAuthors.First(a => a.Id == x.AuthorId)
				});
			_preparedUsers = fixture.PreparedUsers;
			_preparedComments = fixture.PreparedComments.Select(c => new Comment
			{
				Id = c.Id,
				UserId = c.UserId,
				ProductId = c.ProductId,
				Content = c.Content,
				LikesNumber = c.LikesNumber,
				DislikesNumber = c.DislikesNumber,
				Rating = c.Rating,
				User = _preparedUsers.FirstOrDefault(u => u.Id == c.UserId)!
			});
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task GetByIdAsync_ReturnExpectedComment(int Id)
		{
			Thread.Sleep(500);

			//Arrange
			Comment expected = _preparedComments.FirstOrDefault(x => x.Id == Id)!;

			//Act
			Comment comment = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, comment);
		}

		[Theory]
		[InlineData("Mikhail")]
		[InlineData("Bender")]
		[InlineData("Kyle")]
		[InlineData("Nikita")]
		public async Task GetByUserName_ReturnExpectedComment(string name)
		{
			Thread.Sleep(500);

			//Arrange
			IEnumerable<Comment> expected = _preparedComments.Where(x => x.User.Name == name)!;

			//Act
			IEnumerable<Comment> comments = await _repository.GetByUserName(name);

			//Assert
			Assert.Equal(expected, comments);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedCommentsAmount()
		{
			Thread.Sleep(500);

			//Arrange
			int expected = _preparedComments.Count();

			//Act
			IEnumerable<Comment> comments = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, comments.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedCommentsContent()
		{
			Thread.Sleep(500);

			//Arrange
			IEnumerable<Comment> expected = _preparedComments;

			//Act
			IEnumerable<Comment> comment = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, comment);
		}

		[Theory]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		public async Task CreateAsync_AddNewCommentToDb(int caseId)
		{
			//Arrange
			Comment expected = NewComments.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.CreateAsync(expected);
			Comment actualComment = await _repository.GetByIdAsync(caseId);
			expected.User = _preparedUsers.FirstOrDefault(u => u.Id == expected.UserId)!;

			//Assert
			Assert.Equal(expected, actualComment);

			await IntegrationTestHelper.RecreateTable(_client, _preparedUsers);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedProducts);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedComments);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		public async Task UpdateAsync_UpdateCommentContent(int caseId)
		{
			//Arrange
			Comment expected = UpdatedComments.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.UpdateAsync(expected);
			Comment actualComment = await _repository.GetByIdAsync(caseId);
			expected.User = _preparedUsers.FirstOrDefault(u => u.Id == expected.UserId)!;

			//Assert
			Assert.Equal(expected, actualComment);

			await IntegrationTestHelper.RecreateTable(_client, _preparedUsers);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedProducts);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedComments);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(4)]
		public async Task DeleteAsync_RemoveCommentFromDb(int caseId)
		{
			//Arrange
			Comment comment = _preparedComments.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(comment);
			Comment actualComment = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualComment);

			await IntegrationTestHelper.RecreateTable(_client, _preparedUsers);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedProducts);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedComments);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(5)]
		public async Task DeleteByIdAsync_RemoveCommentFromDb(int caseId)
		{
			//Arrange

			//Act
			await _repository.DeleteByIdAsync(caseId);
			Comment actualComment = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualComment);

			await IntegrationTestHelper.RecreateTable(_client, _preparedUsers);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedProducts);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedComments);
		}

		private IEnumerable<Comment> NewComments =>
			new Comment[]
			{
				new() 
				{
					Id = 6,
					UserId = 2,
					ProductId = 3,
					Content = "Very exciting story",
					LikesNumber = 11,
					DislikesNumber = 2,
					Rating = 4.8M
				},
				new() 
				{
					Id = 7,
					UserId = 3,
					ProductId = 2,
					Content = "Recommended by friends, I can not break away",
					LikesNumber = 3,
					DislikesNumber = 0,
					Rating = 5.0M
				},
				new()
				{
					Id = 8,
					UserId = 4,
					ProductId = 1,
					Content = "Not a bad book",
					LikesNumber = 2,
					DislikesNumber = 0,
					Rating = 4.2M
				},
			};

		private IEnumerable<Comment> UpdatedComments =>
			new Comment[]
			{
				new() 
				{
					Id = 1,
					UserId = 3,
					ProductId = 2,
					Content = "Cool book",
					LikesNumber = 8,
					DislikesNumber = 2,
					Rating = 4.7M
				},
				new() 
				{
					Id = 2,
					UserId = 1,
					ProductId = 1,
					Content = "I love this book",
					LikesNumber = 7,
					DislikesNumber = 1,
					Rating = 4.8M
				},
				new()
				{
					Id = 5,
					UserId = 2,
					ProductId = 4,
					Content = "Now i like it",
					LikesNumber = 13, 
					DislikesNumber = 2,
					Rating = 4.7M
				},
			};
	}
}
