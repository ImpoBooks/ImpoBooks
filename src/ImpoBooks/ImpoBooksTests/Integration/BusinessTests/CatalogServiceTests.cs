﻿using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Catalog;
using ImpoBooks.BusinessLogic.Services.Mapping;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.BusinessLogic.Services.Product;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Catalog;
using ImpoBooks.Tests.Integration.Fixtures;
using ImpoBooks.Tests.Integration.Seeds;
using Supabase;


namespace ImpoBooks.Tests.Integration.BusinessTests
{
	[Collection("Integration Tests Collection")]
	public class CatalogServiceTests : IClassFixture<BookSupabaseFixture>
	{
		private readonly Client _client;
		private readonly BookRepository _repository;
		private readonly CatalogService _catalogService;
		private readonly ProductService _productService;
		private readonly BookSupabaseFixture _fixture;
		private IEnumerable<Author> _preparedAuthors;

		public CatalogServiceTests(BookSupabaseFixture fixture)
		{
			_client = fixture.client;
			_fixture = fixture;
			_repository = new(fixture.client);
			_catalogService = new
				(
					_repository,
					new PersonRepository(fixture.client), 
					new AuthorRepository(fixture.client),
					new PublisherRepository(fixture.client),
					new GenreRepository(fixture.client),
					new BookGenreRepository(fixture.client),
					new ProductRepository (fixture.client)
				);
			_productService = new
				(
					new ProductRepository(fixture.client),
					new CommentRepository(fixture.client),
					new UsersRepository(fixture.client)
				);
			_preparedAuthors = AuthorSeeder.PreparedAuthors
				.Select(x => new Author()
				{
					Id = x.Id,
					PersonId = x.PersonId,
					Person = PersonSeeder.PreparedPersons.First(p => p.Id == x.PersonId)
				});
		}

		[Fact]
		public async Task GetBooksAsync_ReturnIsNullError()
		{
			//Arrang
			FilterModel? filter = null;

			//Act
			ErrorOr<IEnumerable<CatalogBookModel>> result = await _catalogService.GetBooksAsync(filter);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.FilterIsNull, result.FirstError);
		}

		[Fact]
		public async Task GetBooksAsync_ReturnBooksNotFoundError()
		{
			//Arrang
			FilterModel? filter = new() { KeyWords = "No metter"};

			//Act
			await _fixture.DisposeAsync();
			ErrorOr<IEnumerable<CatalogBookModel>> result = await _catalogService.GetBooksAsync(filter);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BooksNotFound, result.FirstError);

			await IntegrationTestHelper.RefreshDb
			(
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed
			);
		}

		[Fact]
		public async Task GetBooksAsync_ReturnNoBookMatchesError()
		{
			//Arrang
			FilterModel? filter = new() { KeyWords = "Wrong key word" };

			//Act
			ErrorOr<IEnumerable<CatalogBookModel>> result = await _catalogService.GetBooksAsync(filter);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.NoBookMatches, result.FirstError);
		}

		[Fact]
		public async Task GetBooksAsync_ReturnCorrectResult()
		{
			//Arrang
			FilterModel? filter = new() { KeyWords = "Secret Garden" };
			IEnumerable<CatalogBookModel> expected = ExpectedBooks.Where(b => b.Name == "The Secret Garden");

			//Act
			ErrorOr<IEnumerable<CatalogBookModel>> result = await _catalogService.GetBooksAsync(filter);

			//Assert
			Assert.Equal(expected, result.Value);
		}

		[Fact]
		public async Task GetGenresAsync_ReturnGenresNotFoundError()
		{
			//Arrang

			//Act
			await IntegrationTestHelper.ClearTable<Genre>(_client);
			ErrorOr<IEnumerable<GenreModel>> result = await _catalogService.GetGenresAsync();

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.GenresNotFound, result.FirstError);

			await IntegrationTestHelper.RefreshDb
			(
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed
			);
		}

		[Fact]
		public async Task GetGenresAsync_ReturnCorrectResult()
		{
			//Arrang
			IEnumerable<GenreModel> expected = GenreSeeder.PreparedGenres.Select(g => g.ToGenreModel());

			//Act
			ErrorOr<IEnumerable<GenreModel>> result = await _catalogService.GetGenresAsync();

			//Assert
			Assert.Equal(expected, result.Value);
		}

		[Fact]
		public async Task GetAuthorsAsync_ReturnGenresNotFoundError()
		{
			//Arrang

			//Act
			await IntegrationTestHelper.ClearTable<Author>(_client);
			ErrorOr<IEnumerable<AuthorModel>> result = await _catalogService.GetAuthorsAsync();

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.AuthorsNotFound, result.FirstError);

			await IntegrationTestHelper.RefreshDb
			(
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed
			);
		}

		[Fact]
		public async Task GetAuthorsAsync_ReturnCorrectResult()
		{
			//Arrang
			IEnumerable<AuthorModel> expected = _preparedAuthors.Select(g => g.ToAuthorModel());

			//Act
			ErrorOr<IEnumerable<AuthorModel>> result = await _catalogService.GetAuthorsAsync();

			//Assert
			Assert.Equal(expected, result.Value);
		}

		[Fact]
		public async Task CreateBookAsync_ReturnBookIsNullError()
		{
			//Arrange
			BookModel? book = null;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.CreateBookAsync(book);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BookIsNull, result.FirstError);
		}

		[Fact]
		public async Task CreateBookAsync_ReturnAlreadyExistsError()
		{
			//Arrange
			BookModel book = PreparedBooks.FirstOrDefault(b => b.Name == "The Secret Garden")!;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.CreateBookAsync(book);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.AlreadyExists, result.FirstError);
		}

		[Theory]
		[InlineData("The Time Catchers")]
		[InlineData("Non-existent book")]
		[InlineData("Unknown Title")]
		[InlineData("Test")]
		public async Task CreateBookAsync_ReturnExpectedResult(string bookName)
		{
			//Arrange
			BookModel book = PreparedBooks.FirstOrDefault(b => b.Name == bookName)!;
			CatalogBookModel expected = ExpectedBooks.FirstOrDefault(b => b.Name == bookName)!;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.CreateBookAsync(book);
			ErrorOr<ProductModel> product = await _productService.GetProductAsync(result.Value.Id);
			expected.Id = result.Value.Id;

			//Assert
			Assert.Equal(expected, result.Value);
			Assert.NotNull(product.Value);

			await IntegrationTestHelper.RefreshDb
			(
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed
			);
		}

		[Fact]
		public async Task UpdateBookAsync_ReturnBookIsNullError()
		{
			//Arrange
			BookModel? book = null;
			int id = 1;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.UpdateBookAsync(id, book);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BookIsNull, result.FirstError);
		}

		[Fact]
		public async Task UpdateBookAsync_ReturnBookNotFoundError()
		{
			//Arrange
			BookModel book = PreparedBooks.FirstOrDefault(b => b.Name == "The Time Catchers")!;
			int id = 6;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.UpdateBookAsync(id, book);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BookNotFound, result.FirstError);
		}

		[Theory]
		[InlineData(3, "Da Vinci Code")]
		[InlineData(5, "The Hobbit")]
		[InlineData(2, "Murder on the Orient Express")]
		public async Task UpdateBookAsync_ReturnExpectedResult(int bookId, string newBookName)
		{
			//Arrange
			BookModel book = PreparedBooks.FirstOrDefault(b => b.Name == newBookName)!;
			CatalogBookModel expected = ExpectedBooks.FirstOrDefault(b => b.Name == newBookName)!;
			expected.Id = bookId;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.UpdateBookAsync(bookId, book);

			//Assert
			Assert.Equal(expected, result.Value);

			await IntegrationTestHelper.RefreshDb
			(
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed
			);
		}

		[Fact]
		public async Task DeleteBookAsync_ReturnBookIdIsZeroError()
		{
			//Arrange
			int id = 0;

			//Act
			ErrorOr<Success> result = await _catalogService.DeleteBookAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BookIdIsZero, result.FirstError);
		}

		[Fact]
		public async Task DeleteBookAsync_ReturnBookNotFoundError()
		{
			//Arrange
			int id = 6;

			//Act
			ErrorOr<Success> result = await _catalogService.DeleteBookAsync(id);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BookNotFound, result.FirstError);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(3)]
		public async Task DeleteBookAsync_DeleteBookByProvidedId(int bookId)
		{
			//Arrange
			ErrorOr<IEnumerable<CatalogBookModel>> booksBeforeDel = await _catalogService.GetBooksAsync(new FilterModel());
			int booksCountBeforeDel = booksBeforeDel.Value.Count();
			//Act
			ErrorOr<Success> result = await _catalogService.DeleteBookAsync(bookId);
			ErrorOr<IEnumerable<CatalogBookModel>> booksAfterDel = await _catalogService.GetBooksAsync(new FilterModel());
			int booksCountAfterDel = booksAfterDel.Value.Count();

			//Assert
			Assert.Equal(Result.Success, result.Value);
			Assert.Equal(booksCountBeforeDel - 1, booksCountAfterDel);

			await IntegrationTestHelper.RefreshDb
			(
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed
			);
		}

		private IEnumerable<BookModel> PreparedBooks =>
			new BookModel[]
			{
				new()
				{
					Name = "The Secret Garden",
					Description = "The story of a girl who discovers a magical garden and transforms her life",
					Author = "Andriy Grytsenko",
					Genres = "Detective Adventure",
					Publisher = "Old Lion Publishing House",
					ReleaseDate = "2021.05.2",
					Rating = 4.8M,
					Price = 25.99M
				},
				new()
				{
					Name = "The Time Catchers",
					Description = "A fantasy novel series where heroes travel through time",
					Author = "Kateryna Moroz",
					Genres = "Adventure Myth",
					Publisher = "Nash Format",
					ReleaseDate = "2014.04.23",
					Rating = 4.6M,
					Price = 21.99M
				},
				new()
				{
					Name = "Non-existent book",
					Description = "Description of non-existent book",
					Author = "Joe Biden",
					Genres = "NewGenre",
					Publisher = "NewPublisher",
					ReleaseDate = "2024.04.01",
					Rating = 3.3M,
					Price = 111.11M
				},
				new()
				{
					Name = "Unknown Title",
					Description = "Description of unknown book",
					Author = "Oleksandr Shevchenko",
					Genres = "Detective Science-Fiction",
					Publisher = "Smoloskyp",
					ReleaseDate = "2023.12.15",
					Rating = 4.5M,
					Price = 99.99M
				},
				new()
				{
					Name = "Da Vinci Code",
					Description = "New Description",
					Author = "Oleksandr Shevchenko",
					Genres = "Detective Science-Fiction",
					Publisher = "Smoloskyp",
					ReleaseDate = "2003.03.18",
					Rating = 4.3M,
					Price = 38.99M
				},
				new()
				{
					Name = "The Hobbit",
					Description = "The adventures of Bilbo Baggins in the fantastic world of Middle-earth",
					Author = "Olha Syrenko",
					Genres = "Science-Fiction",
					Publisher = "Ranok",
					ReleaseDate = "1947.03.18",
					Rating = 4.3M,
					Price = 38.99M
				},
				new()
				{
					Name = "Murder on the Orient Express",
					Description = "A classic detective story about an investigation aboard a train",
					Author = "Volodymyr Tkachenko",
					Genres = "Fantasy",
					Publisher = "Ranok",
					ReleaseDate = "2019.08.25",
					Rating = 4.5M,
					Price = 32.99M
				},
				new()
				{
				  Name = "Test",
				  Description = "TestDescription",
				  Author = "Test Author",
				  Genres = "TestGenre",
				  Publisher = "TEST",
				  ReleaseDate = "16.11.2024",
				  Price = 22
				}
			};

		private IEnumerable<CatalogBookModel> ExpectedBooks => new CatalogBookModel[]
		{
			new()
			{
				Id = 1,
				Name = "The Secret Garden",
				Author = "Andriy Grytsenko",
				Genres = "Detective Adventure",
				ReleaseDate = "2021.05.2",
				Rating = 4.8M,
				Price = 25.99M,
			},
			new()
			{
				Name = "The Time Catchers",
				Author = "Kateryna Moroz",
				Genres = "Adventure Myth",
				ReleaseDate = "2014.04.23",
				Rating = 4.6M,
				Price = 21.99M
			},
			new()
			{
				Name = "Non-existent book",
				Author = "Joe Biden",
				Genres = "NewGenre",
				ReleaseDate = "2024.04.01",
				Rating = 3.3M,
				Price = 111.11M
			},
			new()
			{
				Name = "Unknown Title",
				Author = "Oleksandr Shevchenko",
				Genres = "Detective Science-Fiction",
				ReleaseDate = "2023.12.15",
				Rating = 4.5M,
				Price = 99.99M
			},
			new()
			{
				Name = "Da Vinci Code",
				Author = "Oleksandr Shevchenko",
				Genres = "Detective Science-Fiction",
				ReleaseDate = "2003.03.18",
				Rating = 4.3M,
				Price = 38.99M
			},
			new()
			{
				Name = "The Hobbit",
				Author = "Olha Syrenko",
				Genres = "Science-Fiction",
				ReleaseDate = "1947.03.18",
				Rating = 4.3M,
				Price = 38.99M
			},
			new()
			{
				Name = "Murder on the Orient Express",
				Author = "Volodymyr Tkachenko",
				Genres = "Fantasy",
				ReleaseDate = "2019.08.25",
				Rating = 4.5M,
				Price = 32.99M
			},
			new()
				{
				  Name = "Test",
				  Author = "Test Author",
				  Genres = "TestGenre",
				  ReleaseDate = "16.11.2024",
				  Rating = 0,
				  Price = 22
				}
		};
	}
}
