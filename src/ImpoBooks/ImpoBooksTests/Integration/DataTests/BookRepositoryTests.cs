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
	[Collection("Data Tests Collection")]
	public class BookRepositoryTests : IClassFixture<BookSupabaseFixture>
	{
		private readonly Client _client;
		private readonly BookRepository _repository;
		private IEnumerable<Person> _preparedPersons;
		private IEnumerable<Author> _preparedAuthors;
		private IEnumerable<Genre> _preparedGenres;
		private IEnumerable<Publisher> _preparedPublishers;
		private IEnumerable<Book> _preparedBooks;
		private IEnumerable<BookGenre> _prepearedBookGenreRelations;

		public BookRepositoryTests(BookSupabaseFixture fixture)
		{
			_client = fixture.client;
			_repository = new(fixture.client);
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

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task GetByIdAsync_ReturnExpectedBook(int Id)
		{
			//Arrange
			Book expected = _preparedBooks.FirstOrDefault(x => x.Id == Id)!;
			expected = AddGenres(expected);

			//Act
			Book book = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, book);
		}

		[Theory]
		[InlineData("The Hobbit")]
		[InlineData("The Picture of Dorian Gray")]
		[InlineData("Murder on the Orient Express")]
		public async Task GetByNameAsync_ReturnExpectedBook(string name)
		{
			//Arrange
			Book expected = _preparedBooks.FirstOrDefault(x => x.Name == name)!;
			expected = AddGenres(expected);

			//Act
			Book book = await _repository.GetByNameAsync(name);

			//Assert
			Assert.Equal(expected, book);
		}

		[Theory]
		[InlineData(25.99)]
		[InlineData(19.99)]
		[InlineData(22.99)]
		public async Task GetByPriceAsync_ReturnExpectedBooks(decimal price)
		{
			//Arrange
			IEnumerable<Book> expected = _preparedBooks.Where(x => x.Price == price)!;
			expected = expected.Select(x => AddGenres(x));

			//Act
			IEnumerable<Book> books = await _repository.GetByPriceAsync(price);

			//Assert
			Assert.Equal(expected, books);
		}

		[Theory]
		[InlineData(4.7)]
		[InlineData(4.8)]
		[InlineData(4.3)]
		public async Task GetByRaitingAsync_ReturnExpectedBooks(decimal rating)
		{
			//Arrange
			IEnumerable<Book> expected = _preparedBooks.Where(x => x.Rating == rating)!;
			expected = expected.Select(x => AddGenres(x));

			//Act
			IEnumerable<Book> books = await _repository.GetByRaitingAsync(rating);

			//Assert
			Assert.Equal(expected, books);
		}

		[Theory]
		[InlineData("Electronic")]
		[InlineData("Print")]
		public async Task GetByFormatAsync_ReturnExpectedBooks(string format)
		{
			//Arrange
			IEnumerable<Book> expected = _preparedBooks.Where(x => x.Format == format)!;
			expected = expected.Select(x => AddGenres(x));

			//Act
			IEnumerable<Book> books = await _repository.GetByFormatAsync(format);

			//Assert
			Assert.Equal(expected, books);
		}

		[Theory]
		[InlineData("1998.07.12")]
		[InlineData("2003.03.18")]
		[InlineData("2021.05.2")]
		public async Task GetByReleasDateAsync_ReturnExpectedBooks(string releaseDate)
		{
			//Arrange
			IEnumerable<Book> expected = _preparedBooks.Where(x => x.ReleaseDate == releaseDate)!;
			expected = expected.Select(x => AddGenres(x));

			//Act
			IEnumerable<Book> books = await _repository.GetByReleasDateAsync(releaseDate);

			//Assert
			Assert.Equal(expected, books);
		}

		[Theory]
		[InlineData("Old Lion Publishing House")]
		[InlineData("Smoloskyp")]
		[InlineData("Ranok")]
		public async Task GetByPublisherNameAsync_ReturnExpectedBooks(string name)
		{
			//Arrange
			IEnumerable<Book> expected = _preparedBooks.Where(x => x.Publisher.Name == name)!;
			expected = expected.Select(x => AddGenres(x));

			//Act
			IEnumerable<Book> books = await _repository.GetByPublisherNameAsync(name);

			//Assert
			Assert.Equal(expected, books);
		}

		[Theory]
		[InlineData("Oleksandr", "Shevchenko")]
		[InlineData("Volodymyr", "Tkachenko")]
		[InlineData("Andriy", "Grytsenko")]
		public async Task GetByAuthorFullNameAsync_ReturnExpectedBooks(string name, string surname)
		{
			//Arrange
			IEnumerable<Book> expected = _preparedBooks
				.Where(x => x.Author.Person.Name == name &&
							x.Author.Person.Surname == surname)!;
			expected = expected.Select(x => AddGenres(x));

			//Act
			IEnumerable<Book> books = await _repository.GetByAuthorFullNameAsync(name, surname);

			//Assert
			Assert.Equal(expected, books);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedBooksAmount()
		{
			//Arrange
			int expected = _preparedBooks.Count();

			//Act
			IEnumerable<Book> books = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, books.Count());
		}


		[Fact]
		public async Task GetAllAsync_ReturnExpectedBooksContent()
		{
			//Arrange
			IEnumerable<Book> expected = _preparedBooks;
			expected = expected.Select(x => AddGenres(x));

			//Act
			IEnumerable<Book> books = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, books);
		}

		[Theory]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		public async Task CreateAsync_AddNewBookToDb(int caseId)
		{
			//Arrange
			Book expected = NewBooks.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.CreateAsync(expected);
			Book actualBook = await _repository.GetByIdAsync(caseId);
			expected = AddGenres(expected);

			//Assert
			Assert.Equal(expected, actualBook);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		[Theory]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		public async Task UpdateAsync_UpdateBookContent(int caseId)
		{
			//Arrange
			Book expected = UpdatedBooks.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.UpdateAsync(expected);
			Book actualBook = await _repository.GetByIdAsync(caseId);
			expected = AddGenres(expected);

			//Assert
			Assert.Equal(expected, actualBook);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
			Thread.Sleep(2000);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(4)]
		[InlineData(3)]
		public async Task DeleteAsync_RemoveBookFromDb(int caseId)
		{
			//Arrange
			Book book = _preparedBooks.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(book);
			Book actualBook = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualBook);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(5)]
		public async Task DeleteByIdAsync_RemoveBookFromDb(int caseId)
		{
			//Arrange

			//Act
			await _repository.DeleteByIdAsync(caseId);
			Book actualBook = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualBook);

			await IntegrationTestHelper.RecreateTable(_client, _preparedPublishers);
			await IntegrationTestHelper.RecreateTable(_client, _preparedGenres);
			await IntegrationTestHelper.RecreateTable(_client, _preparedPersons);
			await IntegrationTestHelper.RecreateTable(_client, _preparedAuthors);
			await IntegrationTestHelper.RecreateTable(_client, _preparedBooks);
			await IntegrationTestHelper.RecreateTable(_client, _prepearedBookGenreRelations);
		}

		private IEnumerable<Book> NewBooks =>
			new Book[]
			{
				new()
				{
					Id = 6,
					Name = "Influence: The Psychology of Persuasion",
					Description = "Fundamentals of psychology for persuasion and interaction with others",
					ReleaseDate = "2010.01.10",
					Rating = 4.4M,
					Format = "Electronic",
					Price = 12.99M,
					PublisherId = 1,
					AuthorId = 2,
					Publisher = _preparedPublishers.First(p => p.Id == 1),
					Author = _preparedAuthors.First(a => a.Id == 2)
				},
				new()
				{
					Id = 7,
					Name = "The Night Circus",
					Description = "A magical novel about a mysterious circus that appears only at night",
					ReleaseDate = "2011.09.13",
					Rating = 4.5M,
					Format = "Electronic",
					Price = 17.99M,
					PublisherId = 4,
					AuthorId = 5,
					Publisher = _preparedPublishers.First(p => p.Id == 4),
					Author = _preparedAuthors.First(a => a.Id == 5)
				},
				new()
				{
					Id = 8,
					Name = "The Time Catchers",
					Description = "A fantasy novel series where heroes travel through time",
					ReleaseDate = "2014.04.23",
					Rating = 4.6M,
					Format = "Print",
					Price = 21.99M,
					PublisherId = 3,
					AuthorId = 3,
					Publisher = _preparedPublishers.First(p => p.Id == 3),
					Author = _preparedAuthors.First(a => a.Id == 3)
				}
			};

		private IEnumerable<Book> UpdatedBooks =>
			new Book[]
			{
				new()
				{
					Id = 3,
					Name = "The Da Vinci Code",
					Description = "Puzzles, ancient symbols, and intrigue unfolding in the heart of the religious world",
					ReleaseDate = "2001.02.27",
					Rating = 4.4M,
					Format = "Electronic",
					Price = 38.99M,
					PublisherId = 2,
					AuthorId = 1,
					Publisher = _preparedPublishers.First(p => p.Id == 2),
					Author = _preparedAuthors.First(a => a.Id == 1)
				},
				new()
				{
					Id = 4,
					Name = "The Picture of Dorian Gray",
					Description = "A moral tale about a young man obsessed with his beauty",
					ReleaseDate = "1998.07.12",
					Rating = 4.7M,
					Format = "Print",
					Price = 21.99M,
					PublisherId = 5,
					AuthorId = 5,
					Publisher = _preparedPublishers.First(p => p.Id == 5),
					Author = _preparedAuthors.First(a => a.Id == 5)
				},
				new()
				{
					Id = 5,
					Name = "The Hobbit",
					Description = "The adventures of Bilbo Baggins in the fantastic world of Middle-earth",
					ReleaseDate = "1937.09.21",
					Rating = 4.9M,
					Format = "Print",
					Price = 27.99M,
					PublisherId = 3,
					AuthorId = 1,
					Publisher = _preparedPublishers.First(p => p.Id == 3),
					Author = _preparedAuthors.First(a => a.Id == 1)
				}
			};

		private Book AddGenres(Book book)
		{
			ICollection<int> genresIds = _prepearedBookGenreRelations
				.Where(x => x.BookId == book.Id)
				.Select(x => x.GenreId)
				.ToList();
			book.Genres = _preparedGenres
				.Where(x => genresIds.Contains(x.Id))
				.ToList();
			return book;
		}
	}
}
