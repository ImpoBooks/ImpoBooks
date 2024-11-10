using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.DataTests.Fixtures;
using Microsoft.Extensions.Configuration;
using Supabase;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ImpoBooksTests.DataTests
{
    [Collection("Data Tests Collection")]
	public class GenreRepositoryTests : IClassFixture<GenreSupabaseFixture>
	{
		private readonly Client _client;
		private readonly GenreRepository _repository;
		private IEnumerable<Genre> _preparedGenres;

		public GenreRepositoryTests(GenreSupabaseFixture fixture)
        {
			_client = fixture.client;
			_repository = new(fixture.client);
			_preparedGenres = fixture.PrepearedGenres;
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
			Genre actualGenre = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualGenre);

			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
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
			Genre actualGenre = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualGenre);

			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public async Task DeleteAsync_RemoveGenreFromDb(int caseId)
		{
			//Arrange
			Genre genre = _preparedGenres.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(genre);
			Genre actualGenre = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualGenre);

			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
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

			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
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
	}
}
