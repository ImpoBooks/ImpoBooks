using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using ImpoBooks.Tests.Integration.Seeds;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Integration Tests Collection")]
	public class PublisherRepositoryTests : IClassFixture<PublisherSupabaseFixture>
	{
		private readonly PublisherRepository _repository;

		public PublisherRepositoryTests(PublisherSupabaseFixture fixture)
		{
			_repository = new(fixture.client);
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
			Publisher expected = PublisherSeeder.PreparedPublishers.FirstOrDefault(x => x.Id == Id)!;
			expected.Books = BookSeeder.PreparedBooks.Where(x => x.PublisherId == Id).ToList();

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
			Publisher expected = PublisherSeeder.PreparedPublishers.FirstOrDefault(x => x.Name == name)!;
			expected.Books = BookSeeder.PreparedBooks.Where(x => x.PublisherId == expected.Id).ToList();

			//Act
			Publisher publisher = await _repository.GetByNameAsync(name);

			//Assert
			Assert.Equal(expected, publisher);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPublishersAmount()
		{
			//Arrange
			int expected = PublisherSeeder.PreparedPublishers.Count();

			//Act
			IEnumerable<Publisher> publishers = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, publishers.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPublishersContent()
		{
			//Arrange
			IEnumerable<Publisher> expected = PublisherSeeder.PreparedPublishers;
			expected = expected.Select(x => new Publisher()
			{
				Id = x.Id,
				Name = x.Name,
				Books = BookSeeder.PreparedBooks.Where(b => b.PublisherId == x.Id).ToList()
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
			expected.Books = BookSeeder.PreparedBooks.Where(x => x.PublisherId == caseId).ToList();
			Publisher actualPublisher = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualPublisher);

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

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(4)]
		public async Task DeleteAsync_RemovePublisherFromDb(int caseId)
		{
			//Arrange
			Publisher publisher = PublisherSeeder.PreparedPublishers.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(publisher);
			Publisher actualPublisher = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualPublisher);

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
