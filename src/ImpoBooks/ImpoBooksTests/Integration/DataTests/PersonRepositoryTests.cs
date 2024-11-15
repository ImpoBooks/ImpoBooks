using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using Microsoft.Extensions.Configuration;
using Supabase;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Integration Tests Collection")]
	public class PersonRepositoryTests : IClassFixture<PersonSupabaseFixture>
	{
		private readonly Client _client;
		private readonly PersonRepository _repository;
		private IEnumerable<Person> _preparedPersons;

		public PersonRepositoryTests(PersonSupabaseFixture fixture)
		{
			_client = fixture.client;
			_repository = new(fixture.client);
			_preparedPersons = fixture.PreparedPersons;
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(10)]
		public async Task GetByIdAsync_ReturnExpectedPerson(int Id)
		{
			Thread.Sleep(500);

			//Arrange
			Person expected = _preparedPersons.FirstOrDefault(x => x.Id == Id)!;

			//Act
			Person person = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, person);
		}

		[Theory]
		[InlineData("Olha", "Sydenko")]
		[InlineData("Iryna", "Petrenko")]
		[InlineData("Tyler", "Durden")]
		public async Task GetByFullNameAsync_ReturnExpectedPerson(string name, string surname)
		{
			Thread.Sleep(500);

			//Arrange
			IEnumerable<Person> expected = _preparedPersons.Where(x => x.Name == name && x.Surname == surname)!;

			//Act
			IEnumerable<Person> person = await _repository.GetByFullNameAsync(name, surname);

			//Assert
			Assert.Equal(expected, person);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPersonsAmount()
		{
			Thread.Sleep(500);

			//Arrange
			int expected = _preparedPersons.Count();

			//Act
			IEnumerable<Person> persons = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, persons.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPersonsContent()
		{
			Thread.Sleep(500);

			//Arrange
			IEnumerable<Person> expected = _preparedPersons;

			//Act
			IEnumerable<Person> persons = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, persons);
		}

		[Theory]
		[InlineData(11)]
		[InlineData(12)]
		[InlineData(13)]
		[InlineData(14)]
		public async Task CreateAsync_AddNewPersonToDb(int caseId)
		{
			//Arrange
			Person expected = NewPersons.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.CreateAsync(expected);
			Person actualPerson = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualPerson);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(7)]
		[InlineData(1)]
		[InlineData(9)]
		public async Task UpdateAsync_UpdatePesonContent(int caseId)
		{
			//Arrange
			Person expected = UpdatedPersons.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.UpdateAsync(expected);
			Person actualPerson = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualPerson);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(8)]
		public async Task DeleteAsync_RemovePersonFromDb(int caseId)
		{
			//Arrange
			Person person = _preparedPersons.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(person);
			Person actualPerson = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualPerson);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
		}

		[Theory]
		[InlineData(3)]
		[InlineData(7)]
		[InlineData(1)]
		[InlineData(9)]
		public async Task DeleteByIdAsync_RemovePersonFromDb(int caseId)
		{
			//Arrange

			//Act
			await _repository.DeleteByIdAsync(caseId);
			Person actualPerson = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualPerson);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
		}

		private IEnumerable<Person> NewPersons =>
			new Person[]
			{
				new() { Id = 11, Name = "Taras", Surname = "Shevchenko"},
				new() { Id = 12, Name = "Ivan", Surname = "Franko"},
				new() { Id = 13, Name = "Lesya", Surname = "Ukrainka"},
				new() { Id = 14, Name = "Oleh", Surname = "Vynnyk"}
			};

		private IEnumerable<Person> UpdatedPersons =>
			new Person[]
			{
				new() { Id = 5, Name = "Vasyl", Surname = "Stus"},
				new() { Id = 7, Name = "Ivan", Surname = "Franko"},
				new() { Id = 1, Name = "Pavlo", Surname = "Tychyna"},
				new() { Id = 9, Name = "Oles", Surname = "Honchar"}
			};
	}
}
