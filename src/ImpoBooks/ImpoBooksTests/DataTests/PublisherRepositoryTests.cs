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
	public class PublisherRepositoryTests : IClassFixture<PublisherSupabaseFixture>
	{
		private readonly Client _client;
		private readonly PublisherRepository _repository;
		private IEnumerable<Person> _preparedPersons;
		private IEnumerable<Author> _preparedAuthors;
		private IEnumerable<Genre> _preparedGenres;
		private IEnumerable<Publisher> _preparedPublishers;
		private IEnumerable<Book> _preparedBooks;


		public PublisherRepositoryTests(PublisherSupabaseFixture fixture)
        {
			_client = fixture.client;
			_repository = new(fixture.client);
			_preparedPublishers = fixture.PrepearedPublishers;
			_preparedBooks = fixture.PrepearedBooks;
			_preparedAuthors = fixture.PrepearedAuthors;
			_preparedPersons = fixture.PrepearedPersons;
			_preparedGenres = fixture.PrepearedGenres;
		}

        [Theory]
        [InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task GetByIdAsync_ReturnExpectedPublisher(int Id)
        {
			//Arrange
			Publisher expected = _preparedPublishers.FirstOrDefault(x => x.Id == Id)!;
			expected.Books = _preparedBooks.Where(x => x.PublisherId == Id).ToList();

			//Act
			Publisher publisher = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, publisher);
		}

		[Theory]
		[InlineData("Smoloskyp")]
		[InlineData("Old Lion Publishing House")]
		[InlineData("Vivat")]
		public async Task GetByNameAsync_ReturnExpectedPublisher(string name)
		{
			//Arrange
			Publisher expected = _preparedPublishers.FirstOrDefault(x => x.Name == name)!;
			expected.Books = _preparedBooks.Where(x => x.PublisherId == expected.Id).ToList();

			//Act
			Publisher publisher = await _repository.GetByNameAsync(name);

			//Assert
			Assert.Equal(expected, publisher);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPublishersAmount()
		{
			//Arrange
			int expected = _preparedPublishers.Count();

			//Act
			IEnumerable<Publisher> publishers = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, publishers.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPublishersContent()
		{
			//Arrange
			IEnumerable<Publisher> expected = _preparedPublishers;
			expected = expected.Select(x => new Publisher() 
			{ 
				Id = x.Id,
				Name = x.Name, 
				Books = _preparedBooks.Where(b => b.PublisherId == x.Id).ToList() 
			});

			//Act
			IEnumerable<Publisher> publisher = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, publisher);
		}

		[Theory]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		public async Task CreateAsync_AddNewPublisherToDb(int caseId)
		{
			//Arrange
			Publisher expected = NewPublishers.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.CreateAsync(expected);
			Publisher actualPublisher = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualPublisher);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);

		}

		[Theory]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task UpdateAsync_UpdatePublisherContent(int caseId)
		{
			//Arrange
			Publisher expected = UpdatedPublishers.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.UpdateAsync(expected);
			expected.Books = _preparedBooks.Where(x => x.PublisherId == caseId).ToList();
			Publisher actualPublisher = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualPublisher);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(4)]
		public async Task DeleteAsync_RemovePublisherFromDb(int caseId)
		{
			//Arrange
			Publisher publisher = _preparedPublishers.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(publisher);
			Publisher actualPublisher = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualPublisher);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(5)]
		public async Task DeleteByIdAsync_RemovePublisherFromDb(int caseId)
		{
			//Arrange

			//Act
			await _repository.DeleteByIdAsync(caseId);
			Publisher actualPublisher = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualPublisher);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
		}

		private IEnumerable<Publisher> NewPublishers =>
			new Publisher[]
			{
				new() { Id = 6, Name = "Osnovy"},
				new() { Id = 7, Name = "Folio"},
				new() { Id = 8, Name = "KM-Books"}
			};

		private IEnumerable<Publisher> UpdatedPublishers =>
			new Publisher[]
			{
				new() { Id = 3, Name = "Happy day"},
				new() { Id = 4, Name = "Papirus"},
				new() { Id = 5, Name = "Sans"},
			};
	}
}
