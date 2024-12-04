using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using ImpoBooks.Tests.Integration.Seeds;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Integration Tests Collection")]
	public class AuthorRepositoryTests : IClassFixture<AuthorSupabaseFixture>
	{
		private readonly AuthorRepository _repositoryA;
		private readonly PersonRepository _repositoryP;
		private IEnumerable<Author> _preparedAuthors;


		public AuthorRepositoryTests(AuthorSupabaseFixture fixture)
		{
			_repositoryA = new(fixture.client);
			_repositoryP = new(fixture.client);
			_preparedAuthors = AuthorSeeder.PreparedAuthors
				.Select(x => new Author()
				{
					Id = x.Id,
					PersonId = x.PersonId,
					Person = PersonSeeder.PreparedPersons.First(p => p.Id == x.PersonId)
				});
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task GetByIdAsync_ReturnExpectedAuthor(int Id)
		{
			//Arrange
			Author expected = _preparedAuthors.FirstOrDefault(x => x.Id == Id)!;
			expected.Books = BookSeeder.PreparedBooks.Where(x => x.AuthorId == Id).ToList();

			//Act
			Author author = await _repositoryA.GetByIdAsync(expected.Id);
			author.BaseUrl = null;

			//Assert
			Assert.Equal(expected, author);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedAuthorsAmount()
		{
			//Arrange
			int expected = _preparedAuthors.Count();

			//Act
			IEnumerable<Author> author = await _repositoryA.GetAllAsync();

			//Assert
			Assert.Equal(expected, author.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedAuthorsContent()
		{
			//Arrange
			IEnumerable<Author> expected = _preparedAuthors;
			expected = expected.Select(x => new Author()
			{
				Id = x.Id,
				PersonId = x.PersonId,
				Person = x.Person,
				Books = BookSeeder.PreparedBooks.Where(b => b.AuthorId == x.Id).ToList()
			});

			//Act
			IEnumerable<Author> author = await _repositoryA.GetAllAsync();

			//Assert
			Assert.Equal(expected, author);
		}

		[Theory]
		[InlineData("Olha", "Sydenko")]
		[InlineData("Andriy", "Grytsenko")]
		[InlineData("Oleksandr", "Shevchenko")]
		public async Task GetByFullNameAsync_ReturnExpectedAuthor(string name, string surname)
		{
			//Arrange
			Author expected = _preparedAuthors.First(x => x.Person.Name == name && x.Person.Surname == surname);
			expected.Books = BookSeeder.PreparedBooks.Where(x => x.AuthorId == expected.Id).ToList();

			//Act
			Author author = await _repositoryA.GetByFullNameAsync(name, surname);

			//Assert
			Assert.Equal(expected, author);
		}

		[Theory]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		public async Task CreateAsync_AddNewAuthorToDb(int caseId)
		{
			//Arrange
			Author expectedA = NewAuthors.FirstOrDefault(x => x.Id == caseId)!;
			Person expectedP = NewPersons.FirstOrDefault(x => x.Id == expectedA.PersonId)!;
			expectedA.Person = expectedP;

			//Act
			await _repositoryP.CreateAsync(expectedP);
			await _repositoryA.CreateAsync(expectedA);
			Author actualAuthor = await _repositoryA.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expectedA, actualAuthor);

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
		[InlineData(4)]
		[InlineData(5)]
		public async Task UpdateAsync_UpdateAuthorContent(int caseId)
		{
			//Arrange
			Author expected = UpdatedAuthors.FirstOrDefault(a => a.Id == caseId)!;
			expected.Person = PersonSeeder.PreparedPersons.FirstOrDefault(p => p.Id == expected.PersonId)!;

			//Act
			await _repositoryA.UpdateAsync(expected);
			expected.Books = BookSeeder.PreparedBooks.Where(x => x.AuthorId == expected.Id).ToList();
			Author actualAuthor = await _repositoryA.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualAuthor);

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
		[InlineData(4)]
		[InlineData(5)]
		public async Task DeleteAsync_RemoveAuthorFromDb(int caseId)
		{
			//Arrange
			Author author = _preparedAuthors.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repositoryA.DeleteAsync(author);
			Author actualAuthor = await _repositoryA.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualAuthor);

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
		[InlineData(2)]
		[InlineData(1)]
		public async Task DeleteByIdAsync_RemoveAuthorFromDb(int caseId)
		{
			//Arrange

			//Act
			await _repositoryA.DeleteByIdAsync(caseId);
			Author actualAuthor = await _repositoryA.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualAuthor);

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

		private IEnumerable<Author> NewAuthors =>
			new Author[]
			{
				new() {Id = 6, PersonId = 11},
				new() {Id = 7, PersonId = 12},
				new() {Id = 8, PersonId = 13},
			};

		private IEnumerable<Person> NewPersons =>
			new Person[]
			{
				new() { Id = 11, Name = "Joe2", Surname = "Biden2"},
				new() { Id = 12, Name = "Fedir2", Surname = "Denchyk2"},
				new() { Id = 13, Name = "Tyler2", Surname = "Durden2"}
			};

		private IEnumerable<Author> UpdatedAuthors =>
			new Author[]
			{
				new() { Id = 2, PersonId = 1},
				new() { Id = 4, PersonId = 3},
				new() { Id = 5, PersonId = 6},
			};
	}
}