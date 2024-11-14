using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.DataTests.Fixtures;
using ImpoBooksTests;
using Microsoft.Extensions.Configuration;
using Supabase;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Data Tests Collection")]
	public class GenreRepositoryTests : IClassFixture<GenreSupabaseFixture>
	{
		private readonly Client _client;
		private readonly GenreRepository _repository;
		private IEnumerable<Person> _preparedPersons;
		private IEnumerable<Author> _preparedAuthors;
		private IEnumerable<Genre> _preparedGenres;
		private IEnumerable<Publisher> _preparedPublishers;
		private IEnumerable<Book> _preparedBooks;
		private IEnumerable<BookGenre> _prepearedBookGenreRelations;
		public GenreRepositoryTests(GenreSupabaseFixture fixture)
		{
			_client = fixture.client;
			_repository = new(fixture.client);
			_preparedGenres = fixture.PrepearedGenres;
			_preparedPersons = fixture.PrepearedPersons;
			_preparedAuthors = fixture.PrepearedAuthors;
			_preparedPublishers = fixture.PrepearedPublishers;
			_preparedBooks = fixture.PrepearedBooks;
			_prepearedBookGenreRelations = fixture.PrepearedBookGenreRelations;
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task GetByIdAsync_ReturnExpectedGenre(int Id)
		{
			//Arrange
			Genre expected = _preparedGenres.FirstOrDefault(x => x.Id == Id)!;
			expected = AddBooks(expected);

			//Act
			Genre genre = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, genre);
		}

		[Theory]
		[InlineData("Fantasy")]
		[InlineData("Detective")]
		[InlineData("Adventure")]
		public async Task GetByNameAsync_ReturnExpectedGenre(string name)
		{
			//Arrange
			Genre expected = _preparedGenres.FirstOrDefault(x => x.Name == name)!;
			expected = AddBooks(expected);

			//Act
			Genre genre = await _repository.GetByNameAsync(name);

			//Assert
			Assert.Equal(expected, genre);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedGenresAmount()
		{
			//Arrange
			int expected = _preparedGenres.Count();

			//Act
			IEnumerable<Genre> genres = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, genres.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedGenresContent()
		{
			//Arrange
			IEnumerable<Genre> expected = _preparedGenres;
			expected = expected.Select(x => AddBooks(x));

			//Act
			IEnumerable<Genre> genres = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, genres);
		}

		[Theory]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		public async Task CreateAsync_AddNewGenreToDb(int caseId)
		{
			//Arrange
			Genre expected = NewGenres.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.CreateAsync(expected);
			expected = AddBooks(expected);
			Genre actualGenre = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualGenre);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(1)]
		[InlineData(2)]
		public async Task UpdateAsync_UpdateGenreContent(int caseId)
		{
			//Arrange
			Genre expected = UpdatedGenres.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.UpdateAsync(expected);
			expected = AddBooks(expected);
			Genre actualGenre = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualGenre);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public async Task DeleteAsync_RemoveGenreFromDb(int caseId)
		{
			//Arrange
			Genre genre = _preparedGenres.FirstOrDefault(x => x.Id == caseId)!;

			//Actgi
			await _repository.DeleteAsync(genre);
			Genre actualGenre = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualGenre);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		[Theory]
		[InlineData(4)]
		[InlineData(3)]
		[InlineData(5)]
		public async Task DeleteByIdAsync_RemoveGenreFromDb(int caseId)
		{
			//Arrange

			//Act
			await _repository.DeleteByIdAsync(caseId);
			Genre actualGenre = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualGenre);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		private IEnumerable<Genre> NewGenres =>
			new Genre[]
			{
				new() { Id = 6, Name = "Mystery"},
				new() { Id = 7, Name = "Biography"},
				new() { Id = 8, Name = "Self-Help"}
			};

		private IEnumerable<Genre> UpdatedGenres =>
			new Genre[]
			{
				new() { Id = 5, Name = "Non-Fiction"},
				new() { Id = 1, Name = "Historical Novel"},
				new() { Id = 2, Name = "Drama"},
			};

		private Genre AddBooks(Genre genre)
		{
			ICollection<int> bookIds = _prepearedBookGenreRelations
				.Where(x => x.GenreId == genre.Id)
				.Select(x => x.BookId)
				.ToList();
			genre.Books = _preparedBooks
				.Where(x => bookIds.Contains(x.Id))
				.ToList();
			return genre;
		}
	}
}
