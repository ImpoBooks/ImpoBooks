using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Catalog;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Catalog;
using ImpoBooks.Tests.Integration.Fixtures;
using Microsoft.AspNetCore.Http;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.BusinessTests
{
	[Collection("Integration Tests Collection")]
	public class CatalogServiceTests : IClassFixture<BookSupabaseFixture>
	{
		private readonly Client _client;
		private readonly BookRepository _repository;
		private readonly CatalogService _catalogService;
		private readonly BookSupabaseFixture _fixture;
		private IEnumerable<Person> _preparedPersons;
		private IEnumerable<Author> _preparedAuthors;
		private IEnumerable<Genre> _preparedGenres;
		private IEnumerable<Publisher> _preparedPublishers;
		private IEnumerable<Book> _preparedBooks;
		private IEnumerable<BookGenre> _prepearedBookGenreRelations;

		public CatalogServiceTests(BookSupabaseFixture fixture)
		{
			_fixture = fixture;
			_client = fixture.client;
			_repository = new(fixture.client);
			_catalogService = new
				(
					_repository,
					new PersonRepository(fixture.client), 
					new AuthorRepository(fixture.client),
					new PublisherRepository(fixture.client),
					new GenreRepository(fixture.client),
					new BookGenreRepository(fixture.client)
				);
			_preparedPersons = fixture.PrepearedPersons;
			_preparedAuthors = fixture.PrepearedAuthors
				.Select(x => new Author()
				{
					Id = x.Id,
					PersonId = x.PersonId,
					Person = fixture.PrepearedPersons.First(p => p.Id == x.PersonId)
				});
			_preparedGenres = fixture.PrepearedGenres;
			_prepearedBookGenreRelations = fixture.PrepearedBookGenreRelations;
			_preparedPublishers = fixture.PrepearedPublishers;
			_preparedBooks = fixture.PrepearedBooks
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

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
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
		public async Task CreateBookAsync_ReturnExpectedResult(string bookName)
		{
			//Arrange
			BookModel book = PreparedBooks.FirstOrDefault(b => b.Name == bookName)!;
			CatalogBookModel expected = ExpectedBooks.FirstOrDefault(b => b.Name == bookName)!;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.CreateBookAsync(book);
			expected.Id = result.Value.Id;

			//Assert
			Assert.Equal(expected, result.Value);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		[Fact]
		public async Task UpdateBookAsync_ReturnBookIsNullError()
		{
			//Arange
			BookModel? book = null;
			int id = 1;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.UpdateBookAsync(id, book);

			//Asert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BookIsNull, result.FirstError);
		}

		[Fact]
		public async Task UpdateBookAsync_ReturnBookNotFoundError()
		{
			//Arange
			BookModel book = PreparedBooks.FirstOrDefault(b => b.Name == "The Time Catchers")!;
			int id = 6;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.UpdateBookAsync(id, book);

			//Asert
			Assert.True(result.IsError);
			Assert.Equal(CatalogErrors.BookNotFound, result.FirstError);
		}

		[Theory]
		[InlineData(3, "Da Vinci Code")]
		[InlineData(5, "The Hobbit")]
		[InlineData(2, "Murder on the Orient Express")]
		public async Task UpdateBookAsync_ReturnExpectedResult(int bookId, string newBookName)
		{
			//Arange
			BookModel book = PreparedBooks.FirstOrDefault(b => b.Name == newBookName)!;
			CatalogBookModel expected = ExpectedBooks.FirstOrDefault(b => b.Name == newBookName)!;
			expected.Id = bookId;

			//Act
			ErrorOr<CatalogBookModel> result = await _catalogService.UpdateBookAsync(bookId, book);

			//Asert
			Assert.Equal(expected, result.Value);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
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
			}
		};
	}
}
