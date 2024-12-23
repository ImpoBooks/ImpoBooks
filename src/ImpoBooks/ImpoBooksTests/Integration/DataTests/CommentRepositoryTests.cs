using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using ImpoBooks.Tests.Integration.Seeds;
using Supabase;
using Supabase.Postgrest.Responses;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Integration Tests Collection")]
	public class CommentRepositoryTests : IClassFixture<CommentSupabaseFIxture>
	{
		private readonly CommentRepository _repository;
		private IEnumerable<Comment> _preparedComments;
		private readonly Client _client;


		public CommentRepositoryTests(CommentSupabaseFIxture fixture)
		{
			_client = fixture.client;
			_repository = new(fixture.client);
			_preparedComments = CommentSeeder.PreparedComments.Select(c => new Comment
			{
				Id = c.Id,
				UserId = c.UserId,
				ProductId = c.ProductId,
				Content = c.Content,
				LikesNumber = c.LikesNumber,
				DislikesNumber = c.DislikesNumber,
				Rating = c.Rating,
				User = UserSeeder.PreparedUsers.FirstOrDefault(u => u.Id == c.UserId)!
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
			//Arrange
			Comment expected = _preparedComments.FirstOrDefault(x => x.Id == Id)!;

			//Act
			Comment comment = await _repository.GetByIdAsync(expected.Id);

			ModeledResponse<Comment> response = await _client.From<Comment>().Get();
			IEnumerable<Comment> entity = response.Models;

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
			expected.User = UserSeeder.PreparedUsers.FirstOrDefault(u => u.Id == expected.UserId)!;

			//Assert
			Assert.Equal(expected, actualComment);

			await IntegrationTestHelper.RefreshDb
			(
				UserSeeder.Seed +
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed +
				ProductSeeder.Seed +
				CommentSeeder.Seed
			);
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
			expected.User = UserSeeder.PreparedUsers.FirstOrDefault(u => u.Id == expected.UserId)!;

			//Assert
			Assert.Equal(expected, actualComment);

			await IntegrationTestHelper.RefreshDb
			(
				UserSeeder.Seed +
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed +
				ProductSeeder.Seed +
				CommentSeeder.Seed 
			);
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

			await IntegrationTestHelper.RefreshDb
			(
				UserSeeder.Seed +
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed +
				ProductSeeder.Seed +
				CommentSeeder.Seed 
			);
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

			await IntegrationTestHelper.RefreshDb
			(
				UserSeeder.Seed +
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed +
				ProductSeeder.Seed +
				CommentSeeder.Seed 
			);
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
