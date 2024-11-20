using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.BusinessLogic.Services.Product;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Catalog;
using ImpoBooks.Infrastructure.Errors.Product;
using ImpoBooks.Tests.Integration.Fixtures;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.BusinessTests
{
	[Collection("Integration Tests Collection")]
	public class ProductServiceTests : IClassFixture<ProductSupabaseFixture>
	{
		private readonly ProductSupabaseFixture _fixture;
		private readonly Client _client;
		private readonly ProductService _productService;
		private readonly ProductRepository _repository;
		private IEnumerable<Person> _preparedPersons;
		private IEnumerable<Author> _preparedAuthors;
		private IEnumerable<Genre> _preparedGenres;
		private IEnumerable<Publisher> _preparedPublishers;
		private IEnumerable<Book> _preparedBooks;
		private IEnumerable<BookGenre> _preparedBookGenreRelations;
		private IEnumerable<User> _preparedUsers;
		private IEnumerable<Comment> _preparedComments;
		private IEnumerable<Product> _preparedProducts;

		public ProductServiceTests(ProductSupabaseFixture fixture)
		{
			_fixture = fixture;
			_client = fixture.client;
			_repository = new(fixture.client);
			_productService = new
				(
					_repository!
				);
			_preparedPersons = fixture.PreparedPersons;
			_preparedAuthors = fixture.PreparedAuthors
				.Select(x => new Author()
				{
					Id = x.Id,
					PersonId = x.PersonId,
					Person = fixture.PreparedPersons.First(p => p.Id == x.PersonId)
				});
			_preparedGenres = fixture.PreparedGenres;
			_preparedBookGenreRelations = fixture.PreparedBookGenreRelations;
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
			_preparedProducts = fixture.PreparedProducts.Select(p => new Product()
			{
				Id = p.Id,
				BookId = p.BookId,
				Book = _preparedBooks.FirstOrDefault(b => b.Id == p.BookId)!,
				Comments = _preparedComments.Where(c => c.ProductId == p.Id).ToList()
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
					Author = "Olha Syrenko",
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
	}
}
