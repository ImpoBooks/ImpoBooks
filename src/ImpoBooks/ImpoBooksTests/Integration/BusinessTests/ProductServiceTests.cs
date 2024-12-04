using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Mapping;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.BusinessLogic.Services.Product;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Product;
using ImpoBooks.Tests.Integration.Fixtures;
using ImpoBooks.Tests.Integration.Seeds;

namespace ImpoBooks.Tests.Integration.BusinessTests
{
	[Collection("Integration Tests Collection")]
	public class ProductServiceTests : IClassFixture<ProductSupabaseFixture>
	{
		private readonly ProductService _productService;
		private readonly ProductRepository _repository;
		private IEnumerable<Comment> _preparedComments;

		public ProductServiceTests(ProductSupabaseFixture fixture)
		{
			_repository = new(fixture.client);
			_productService = new
				(
					_repository!,
					new CommentRepository(fixture.client),
					new UsersRepository(fixture.client)
				);
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

		[Fact]
		public async Task GetProductAsync_ReturnProductIdIsZeroError()
		{
			//Arrang
			int id = 0;

			//Act
			ErrorOr<ProductModel> result = await _productService.GetProductAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.ProductIdIsZero, result.FirstError);
		}

		[Fact]
		public async Task GetProductAsync_ReturnProductNotFoundError()
		{
			//Arrang
			int id = 7;

			//Act
			ErrorOr<ProductModel> result = await _productService.GetProductAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.ProductNotFound, result.FirstError);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		public async Task GetProductAsync_ReturnExpectedResult(int caseId)
		{
			//Arrang
			ProductModel expected = ExpectedProducts.FirstOrDefault(p => p.Id == caseId)!;

			//Act
			ErrorOr<ProductModel> result = await _productService.GetProductAsync(caseId);

			//Assert
			Assert.Equal(expected, result.Value);
		}

		[Fact]
		public async Task AddCommentAsync_ReturnProductIdIsZeroError()
		{
			//Arrang
			int id = 0;
			ProductCommentModel comment = new ProductCommentModel();

			//Act
			ErrorOr<CommentModel> result = await _productService.AddCommentAsync(id, comment);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.ProductIdIsZero, result.FirstError);
		}

		[Fact]
		public async Task AddCommentAsync_ReturnProductNotFoundError()
		{
			//Arrang
			int id = 6;
			ProductCommentModel comment = new ProductCommentModel();

			//Act
			ErrorOr<CommentModel> result = await _productService.AddCommentAsync(id, comment);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.ProductNotFound, result.FirstError);
		}

		[Fact]
		public async Task AddCommentAsync_ReturnCommentIsNullError()
		{
			//Arrang
			int id = 2;
			ProductCommentModel? comment = null;

			//Act
			ErrorOr<CommentModel> result = await _productService.AddCommentAsync(id, comment!);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.CommentIsNull, result.FirstError);
		}

		[Fact]
		public async Task AddCommentAsync_ReturnCommentContentWrongInfoError()
		{
			//Arrang
			int id = 4;
			ProductCommentModel comment = new() 
			{
				UserId = 3,
				Content = ""
			};

			//Act
			ErrorOr<CommentModel> result = await _productService.AddCommentAsync(id, comment!);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.CommentContentWrongInfo, result.FirstError);
		}

		[Fact]
		public async Task AddCommentAsync_ReturnUserNotFoundError()
		{
			//Arrang
			int id = 3;
			ProductCommentModel comment = new()
			{
				UserId = 10,
				Content = "It`s just a comment"
			};

			//Act
			ErrorOr<CommentModel> result = await _productService.AddCommentAsync(id, comment!);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.UserNotFound, result.FirstError);
		}

		[Theory]
		[InlineData(2, 0)]
		[InlineData(3, 1)]
		[InlineData(5, 2)]
		public async Task AddCommentAsync_ReturnExpectedResult(int productId, int caseId)
		{
			//Arrang
			ProductCommentModel comment = PreparedComments[caseId];
			CommentModel expected = ExpectedComments[caseId];

			//Act
			ErrorOr<CommentModel> result = await _productService.AddCommentAsync(productId, comment!);
			expected.Id = result.Value.Id;

			//Assert
			Assert.Equal(expected, result.Value);

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

		[Fact]
		public async Task IncrementLikeNumberAsync_ReturnCommentIdIsZeroError()
		{
			//Arrang
			int id = 0;

			//Act
			ErrorOr<CommentModel> result = await _productService.IncrementLikeNumberAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.CommentIdIsZero, result.FirstError);
		}

		[Fact]
		public async Task IncrementLikeNumberAsync_ReturnCommentNotFoundError()
		{
			//Arrang
			int id = 8;

			//Act
			ErrorOr<CommentModel> result = await _productService.IncrementLikeNumberAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.CommentNotFound, result.FirstError);
		}

		[Theory] 
		[InlineData(5)]
		[InlineData(3)]
		[InlineData(1)]
		public async Task IncrementLikeNumberAsync_ReturnExpectedResult(int commentId)
		{
			//Arrang
			CommentModel expected = _preparedComments
				.FirstOrDefault(c => c.Id == commentId)!
				.ToCommentModel();
			expected.LikesNumber += 1;

			//Act
			ErrorOr<CommentModel> result = await _productService.IncrementLikeNumberAsync(commentId);

			//Assert
			Assert.Equal(expected, result.Value);

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

		[Fact]
		public async Task IncrementDislikeNumberAsync_ReturnCommentIdIsZeroError()
		{
			//Arrang
			int id = 0;

			//Act
			ErrorOr<CommentModel> result = await _productService.IncrementDislikeNumberAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.CommentIdIsZero, result.FirstError);
		}

		[Fact]
		public async Task IncrementDislikeNumberAsync_ReturnCommentNotFoundError()
		{
			//Arrang
			int id = 9;

			//Act
			ErrorOr<CommentModel> result = await _productService.IncrementDislikeNumberAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(ProductErrors.CommentNotFound, result.FirstError);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(1)]
		public async Task IncrementDislikeNumberAsync_ReturnExpectedResult(int commentId)
		{
			//Arrang
			CommentModel expected = _preparedComments
				.FirstOrDefault(c => c.Id == commentId)!
				.ToCommentModel();
			expected.DislikesNumber += 1;

			//Act
			ErrorOr<CommentModel> result = await _productService.IncrementDislikeNumberAsync(commentId);

			//Assert
			Assert.Equal(expected, result.Value);

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

		private IEnumerable<ProductModel> ExpectedProducts =>
			new ProductModel[]
			{
				new()
				{
					Id = 1,
					Name = "The Secret Garden",
					Author = "Andriy Grytsenko",
					Description = "The story of a girl who discovers a magical garden and transforms her life",
					Format = "Electronic", 
					Genres = "Detective Adventure",
					Publisher = "Old Lion Publishing House",
					ReleaseDate = "2021.05.2",
					Rating = 4.8M,
					Price = 25.99M,
					Comments = new CommentModel[]
					{
						new()
						{
							Id = 1,
							UserName = "Mikhail",
							Content = "Cool book",
							LikesNumber = 7,
							DislikesNumber = 1,
							Rating = 4.5M
						}
					}
				},
				new()
				{
					Id = 2,
					Name = "Murder on the Orient Express",
					Author = "Volodymyr Tkachenko",
					Description = "A classic detective story about an investigation aboard a train",
					Format = "Print",
					Genres = "Science-Fiction",
					Publisher = "Nash Format",
					ReleaseDate = "2019.08.25",
					Rating = 4.6M,
					Price = 19.99M,
					Comments = new CommentModel[]
					{
						new()
						{
							Id = 2,
							UserName = "Bender",
							Content = "I love this book",
							LikesNumber = 9,
							DislikesNumber = 0,
							Rating = 4.7M
						},
						new()
						{
							Id = 5,
							UserName = "Kyle",
							Content = "I don`t like it",
							LikesNumber = 1,
							DislikesNumber = 5,
							Rating = 3.3M
						}
					}
				},
				new()
				{
					Id = 5,
					Name = "The Hobbit",
					Author = "Olha Sydenko",
					Description = "The adventures of Bilbo Baggins in the fantastic world of Middle-earth",
					Format = "Print",
					Genres = "Science-Fiction",
					Publisher = "Smoloskyp",
					ReleaseDate = "1937.09.21",
					Rating = 4.9M,
					Price = 27.99M,
					Comments = new CommentModel[] {}
				},
			};

		private List<ProductCommentModel> PreparedComments =>
			new List<ProductCommentModel>
			{
				new() { UserId = 2, Content = "Not bad", Rating = 4.2M },
				new() { UserId = 4, Content = "Interesting", Rating = 4.0M },
				new() { UserId = 3, Content = "Very exciting book", Rating = 4.7M },
			};
		private List<CommentModel> ExpectedComments =>
			new List<CommentModel>
			{
				new() { UserName = "Bender", Content = "Not bad", LikesNumber = 0, DislikesNumber = 0, Rating = 4.2M },
				new() { UserName = "Nikita", Content = "Interesting", LikesNumber = 0, DislikesNumber = 0, Rating = 4.0M },
				new() { UserName = "Kyle", Content = "Very exciting book", LikesNumber = 0, DislikesNumber = 0, Rating = 4.7M},
			};
	}
}
