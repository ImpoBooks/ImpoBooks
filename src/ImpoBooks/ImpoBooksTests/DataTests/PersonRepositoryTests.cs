using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Supabase;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ImpoBooksTests.DataTests
{
	public class PersonRepositoryTests
    {
		private static readonly Client _client = IntegrationTestHelper.TestClientInit();
		private static readonly PersonRepository _repository = new(_client);

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
			//Arrange
			Person expected = PreparedPersons.FirstOrDefault(x => x.Id == Id)!;

			//Act
			Person person = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, person);
		}

		[Theory]
		[InlineData("Ольга", "Сидоренко")]
		[InlineData("Ірина", "Петренко")]
		[InlineData("Тайлер", "Дерден")]
		public async Task GetByFullNameAsync_ReturnExpectedPerson(string name, string surname)
		{
			//Arrange
			IEnumerable<Person> expected = PreparedPersons.Where(x => x.Name == name && x.Surnmae == surname)!;

			//Act
			IEnumerable<Person> person = await _repository.GetByFullNameAsync(name, surname);

			//Assert
			Assert.Equal(expected, person);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPersonsAmount()
		{
			//Arrange
			int expected = PreparedPersons.Count();

			//Act
			IEnumerable<Person> persons = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, persons.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedPersonsContent()
		{
			//Arrange
			IEnumerable<Person> expected = PreparedPersons;

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

			await IntegrationTestHelper.RecreateTable(_client, PreparedPersons);
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

			await IntegrationTestHelper.RecreateTable(_client, PreparedPersons);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(8)]
		public async Task DeleteAsync_RemovePersonFromDb(int caseId)
		{
			//Arrange
			Person person = PreparedPersons.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(person);
			Person actualPerson = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualPerson);

			await IntegrationTestHelper.RecreateTable(_client, PreparedPersons);
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

			await IntegrationTestHelper.RecreateTable(_client, PreparedPersons);
		}

		private IEnumerable<Person> PreparedPersons =>
            new Person[] 
            { 
                new() { Id = 1, Name = "Олександр", Surnmae = "Шевченко"},
                new() { Id = 2, Name = "Дмитро", Surnmae = "Ковальчук"},
				new() { Id = 3, Name = "Андрій", Surnmae = "Гриценко"},
				new() { Id = 4, Name = "Володимир", Surnmae = "Ткаченко"},
				new() { Id = 5, Name = "Катерина", Surnmae = "Мороз"},
				new() { Id = 6, Name = "Ольга", Surnmae = "Сидоренко"},
				new() { Id = 7, Name = "Ірина", Surnmae = "Петренко"},
				new() { Id = 8, Name = "Джо", Surnmae = "Байден"},
				new() { Id = 9, Name = "Федір", Surnmae = "Денчик"},
				new() { Id = 10, Name = "Тайлер", Surnmae = "Дерден"}
			};

		private IEnumerable<Person> NewPersons =>
			new Person[]
			{
				new() { Id = 11, Name = "Тарас", Surnmae = "Шевченко"},
				new() { Id = 12, Name = "Іван", Surnmae = "Франко"},
				new() { Id = 13, Name = "Леся", Surnmae = "Українка"},
				new() { Id = 14, Name = "Олег", Surnmae = "Вінник"}
			};

		private IEnumerable<Person> UpdatedPersons => 
			new Person[] 
			{
				new() { Id = 5, Name = "Василь", Surnmae = "Стус"},
				new() { Id = 7, Name = "Іван", Surnmae = "Франко"},
				new() { Id = 1, Name = "Павло", Surnmae = "Тичина"},
				new() { Id = 9, Name = "Олесь ", Surnmae = "Гончар"}
			};
	}
}
