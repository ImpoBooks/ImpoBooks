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
			_catalogService = new(_repository);
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
			Assert.Equal(CatalogErrors.IsNull, result.FirstError);
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
			IEnumerable<CatalogBookModel> expected = expectedBooks;

			//Act
			ErrorOr<IEnumerable<CatalogBookModel>> result = await _catalogService.GetBooksAsync(filter);

			//Assert
			Assert.Equal(expected, result.Value);
		}

		private IEnumerable<CatalogBookModel> expectedBooks => new CatalogBookModel[]
		{
			new()
			{
				Id = 1,
				Name = "The Secret Garden",
				Author = "Andriy Grytsenko",
				Genres = "Detective, Adventure",
				ReleaseDate = "2021.05.2",
				Rating = 4.8M,
				Price = 25.99M,
			}
		};
		

	}
}
