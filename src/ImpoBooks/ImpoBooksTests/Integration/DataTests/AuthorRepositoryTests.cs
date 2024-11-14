using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Data Tests Collection")]
	public class AuthorRepositoryTests : IClassFixture<AuthorSupabaseFixture>
	{
		private readonly Client _client;
		private readonly AuthorRepository _repositoryA;
		private readonly PersonRepository _repositoryP;
		private IEnumerable<Person> _preparedPersons;
		private IEnumerable<Author> _preparedAuthors;
		private IEnumerable<Genre> _preparedGenres;
		private IEnumerable<Publisher> _preparedPublishers;
		private IEnumerable<Book> _preparedBooks;


		public AuthorRepositoryTests(AuthorSupabaseFixture fixture)
		{
			_client = fixture.client;
			_repositoryA = new(fixture.client);
			_repositoryP = new(fixture.client);
			_preparedBooks = fixture.PrepearedBooks;
			_preparedGenres = fixture.PrepearedGenres;
			_preparedPublishers = fixture.PrepearedPublishers;
			_preparedPersons = fixture.PrepearedPersons;
			_preparedAuthors = fixture.PrepearedAuthors
				.Select(x => new Author()
				{
					Id = x.Id,
					PersonId = x.PersonId,
					Person = fixture.PrepearedPersons.First(p => p.Id == x.PersonId)
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
			expected.Books = _preparedBooks.Where(x => x.AuthorId == Id).ToList();

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
				Books = _preparedBooks.Where(b => b.AuthorId == x.Id).ToList()
			});

			//Act
			IEnumerable<Author> author = await _repositoryA.GetAllAsync();

			//Assert
			Assert.Equal(expected, author);
		}

		[Theory]
		[InlineData("Olha", "Syrenko")]
		[InlineData("Andriy", "Grytsenko")]
		[InlineData("Oleksandr", "Shevchenko")]
		public async Task GetByFullNameAsync_ReturnExpectedAuthor(string name, string surname)
		{
			//Arrange
			Author expected = _preparedAuthors.First(x => x.Person.Name == name && x.Person.Surname == surname);
			expected.Books = _preparedBooks.Where(x => x.AuthorId == expected.Id).ToList();

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

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task UpdateAsync_UpdateAuthorContent(int caseId)
		{
			//Arrange
			Author expected = UpdatedAuthors.FirstOrDefault(a => a.Id == caseId)!;
			expected.Person = _preparedPersons.FirstOrDefault(p => p.Id == expected.PersonId)!;

			//Act
			await _repositoryA.UpdateAsync(expected);
			expected.Books = _preparedBooks.Where(x => x.AuthorId == expected.Id).ToList();
			Author actualAuthor = await _repositoryA.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualAuthor);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			Thread.Sleep(2000);
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

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
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

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
		}



		private IEnumerable<Author> NewAuthors =>
			new Author[]
			{
				new() {Id = 6, PersonId = 8},
				new() {Id = 7, PersonId = 5},
				new() {Id = 8, PersonId = 7},
			};

		private IEnumerable<Person> NewPersons =>
			new Person[]
			{
				new() { Id = 5, Name = "Joe", Surname = "Biden"},
				new() { Id = 7, Name = "Fedir", Surname = "Denchyk"},
				new() { Id = 8, Name = "Tyler", Surname = "Durden"}
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