using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using Microsoft.Extensions.Configuration;
using Supabase;
using System.Linq;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Integration Tests Collection")]
	public class ProductRepositoryTests : IClassFixture<ProductSupabaseFixture>
	{
		private readonly ProductSupabaseFixture _fixture;
		private readonly Client _client;
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



		public ProductRepositoryTests(ProductSupabaseFixture fixture)
		{
			_fixture = fixture;
			_client = fixture.client;
			_repository = new(fixture.client);
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

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task GetByIdAsync_ReturnExpectedProduct(int Id)
		{
			//Arrange
			Product expected = _preparedProducts.FirstOrDefault(x => x.Id == Id)!;
			expected.Book = AddGenres(expected.Book);

			//Act
			Product product = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, product);
		}

		[Theory]
		[InlineData("The Da Vinci Code", 3)]
		[InlineData("The Hobbit", 5)]
		[InlineData("The Secret Garden", 1)]
		public async Task GetByNameAsync_ReturnExpectedProduct(string name, int caseId)
		{
			//Arrange
			Product expected = _preparedProducts.FirstOrDefault(x => x.Id == caseId)!;
			expected.Book = AddGenres(expected.Book);

			//Act
			Product product = await _repository.GetByNameAsync(name);

			//Assert
			Assert.Equal(expected, product);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedProductsAmount()
		{
			//Arrange
			int expected = _preparedProducts.Count();

			//Act
			IEnumerable<Product> products = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, products.Count());
		}


		[Fact]
		public async Task GetAllAsync_ReturnExpectedProductsContent()
		{
			//Arrange
			IEnumerable<Product> expected = _preparedProducts;
			expected = expected.Select(p => new Product() 
			{
				Id = p.Id,
				BookId = p.BookId,
				Book = AddGenres(p.Book),
				Comments = p.Comments 
			});

			//Act
			IEnumerable<Product> products = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, products);
		}

		[Theory]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		public async Task CreateAsync_AddNewProductToDb(int caseId)
		{
			//Arrange
			Product expected = NewProducts.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.CreateAsync(expected);
			Product actuaProduct = await _repository.GetByIdAsync(caseId);
			expected.Book = AddGenres(_preparedBooks.FirstOrDefault(b => b.Id == expected.BookId)!);

			//Assert
			Assert.Equal(expected, actuaProduct);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBookGenreRelations);
			await IntegrationTestHelper.RecreateTable(_client, _preparedUsers);
			await IntegrationTestHelper.RecreateTable(_client, _fixture.PreparedProducts);
			await IntegrationTestHelper.RecreateTable(_client, _preparedComments);
		}

		private IEnumerable<Product> NewProducts =>
			new Product[]
			{
				new() { Id = 6, BookId = 2},
				new() { Id = 7, BookId = 5},
				new() { Id = 8, BookId = 4}
			};

		private Book AddGenres(Book book)
		{
			ICollection<int> genresIds = _preparedBookGenreRelations
				.Where(x => x.BookId == book.Id)
				.Select(x => x.GenreId)
				.ToList();
			book.Genres = _preparedGenres
				.Where(x => genresIds.Contains(x.Id))
				.ToList();
			return book;
		}
	}
}
