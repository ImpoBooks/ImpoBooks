using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpoBooks.BusinessLogic.Services.Extensions;

namespace ImpoBooks.Tests.Unit.BusinessTests
{
	public class IEnumerableBookExtensionTests
	{

		[Theory]
		[InlineData("", 0)]
		[InlineData("Haryy", 1)]
		[InlineData("Secret Garden", 2)]
		[InlineData("Orient Express", 3)]
		[InlineData("Da Vinci Code", 4)]
		[InlineData("Hobbit", 6)]
		[InlineData("Volodymyr", 7)]
		[InlineData("Olha Syrenko", 8)]
		[InlineData("Shevchenko", 5)]
		[InlineData("Detective", 9)]
		[InlineData("Science Fiction", 10)]
		[InlineData("Fantasy", 6)]
		public void FilterByKeyWord_ReturnExpectedBooks(string keyWord, int caseId)
		{
			//Arang
			IEnumerable<Book> expected = ExpectedBooks[caseId];
			IEnumerable<Book> books = PreparedBooks;

			//Act
			IEnumerable<Book> filteredBooks = books.FilterByKeyWord(keyWord);

			//Assert
			Assert.Equal(expected, filteredBooks);
		}

		[Theory]
		[InlineData("", 0)]
		[InlineData("Detective", 9)]
		[InlineData("Science Fiction", 10)]
		[InlineData("Fantasy", 6)]
		public void FilterByGenre_ReturnExpectedBooks(string genre, int caseId)
		{
			//Arang
			IEnumerable<Book> expected = ExpectedBooks[caseId];
			IEnumerable<Book> books = PreparedBooks;

			//Act
			IEnumerable<Book> filteredBooks = books.FilterByGenre(genre);

			//Assert
			Assert.Equal(expected, filteredBooks);
		}

		[Theory]
		[InlineData("", 0)]
		[InlineData("Volodymyr Tkachenko", 7)]
		[InlineData("Olha Syrenko", 8)]
		[InlineData("Oleksandr Shevchenko", 5)]
		public void FilterByAuthor_ReturnExpectedBooks(string author, int caseId)
		{
			//Arang
			IEnumerable<Book> expected = ExpectedBooks[caseId];
			IEnumerable<Book> books = PreparedBooks;

			//Act
			IEnumerable<Book> filteredBooks = books.FilterByAuthor(author);

			//Assert
			Assert.Equal(expected, filteredBooks);
		}

		[Theory]
		[InlineData(0, 100, 0)]
		[InlineData(0, 25, 11)]
		[InlineData(20, 28, 12)]
		[InlineData(26, 40, 13)]
		public void FilterByPrice_ReturnExpectedBooks(decimal min, decimal max, int caseId)
		{
			//Arang
			IEnumerable<Book> expected = ExpectedBooks[caseId];
			IEnumerable<Book> books = PreparedBooks;

			//Act
			IEnumerable<Book> filteredBooks = books.FilterByPrice(min, max);

			//Assert
			Assert.Equal(expected, filteredBooks);
		}

		[Theory]
		[InlineData(0, 5, 0)]
		[InlineData(4.8, 4.9, 8)]
		[InlineData(4, 4.6, 10)]
		[InlineData(4.5, 4.8, 14)]
		public void FilterByRating_ReturnExpectedBooks(decimal min, decimal max, int caseId)
		{
			//Arang
			IEnumerable<Book> expected = ExpectedBooks[caseId];
			IEnumerable<Book> books = PreparedBooks;

			//Act
			IEnumerable<Book> filteredBooks = books.FilterByRating(min, max);

			//Assert
			Assert.Equal(expected, filteredBooks);
		}

		private List<List<Book>> ExpectedBooks =>
			new List<List<Book>>()
			{
				new List<Book>() {
					PreparedBooks.FirstOrDefault(b => b.Id == 1)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 2)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 3)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 4)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 5)!
				},
				new List<Book>(),
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 1)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 2)!
				},
				new List<Book>() 
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 3)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 4)!
				},
				new List<Book>() 
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 5)!
				},
				new List<Book>() 
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 2)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 3)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 1)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 5)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 1)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 4)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 2)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 3)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 2)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 4)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 1)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 4)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 5)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 3)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 5)!
				},
				new List<Book>()
				{
					PreparedBooks.FirstOrDefault(b => b.Id == 1)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 2)!,
					PreparedBooks.FirstOrDefault(b => b.Id == 4)!
				},
			};

		private List<Book> PreparedBooks =>
			new List<Book>
			{
				new()
				{
					Id = 1,
					Name = "The Secret Garden",
					Description = "The story of a girl who discovers a magical garden and transforms her life",
					ReleaseDate = "2021.05.2",
					Rating = 4.8M,
					Format = "Electronic",
					Price = 25.99M,
					Publisher = new()
					{
						Id = 3,
						Name = "Old Lion Publishing House"
					},
					Author = new()
					{
						Id = 4,
						Person = new()
						{
							Id = 6,
							Name = "Olha",
							Surname = "Syrenko"
						},
					},
					Genres = new Genre[] 
					{
						new() { Id = 2, Name = "Detective" },
						new() { Id = 4, Name = "Adventure"}
					}
				},

				new()
				{
					Id = 2,
					Name = "Murder on the Orient Express",
					Description = "A classic detective story about an investigation aboard a train",
					ReleaseDate = "2019.08.25",
					Rating = 4.6M,
					Format = "Print",
					Price = 19.99M,
					Publisher = new()
					{
						Id = 4,
						Name = "Nash Format",
					},
					Author = new()
					{
						Id = 1,
						Person = new()
						{
							Id = 4,
							Name = "Volodymyr",
							Surname = "Tkachenko"
						},
					},
					Genres = new Genre[]
					{
						new() { Id = 1, Name = "Science Fiction"}
					}
				},
				new()
				{
					Id = 3,
					Name = "The Da Vinci Code",
					Description = "Puzzles, ancient symbols, and intrigue unfolding in the heart of the religious world",
					ReleaseDate = "2003.03.18",
					Rating = 4.3M,
					Format = "Electronic",
					Price = 38.99M,
					Publisher = new()
					{
						Id = 2,
						Name = "Smoloskyp"
					},
					Author = new()
					{
						Id = 1,
						Person = new()
						{
							Id = 4,
							Name = "Volodymyr",
							Surname = "Tkachenko"
						},
					},
					Genres = new Genre[]
					{
						new() { Id = 4, Name = "Adventure"},
						new() { Id = 1, Name = "Science Fiction"},
					}
				},
				new()
				{
					Id = 4,
					Name = "The Picture of Dorian Gray",
					Description = "A moral tale about a young man obsessed with his beauty",
					ReleaseDate = "1998.07.12",
					Rating = 4.7M,
					Format = "Electronic",
					Price = 22.99M,
					Publisher = new()
					{
						 Id = 5,
						Name = "Vivat"
					},
					Author = new()
					{
						Id = 5,
						Person = new()
						{
							Id = 1,
							Name = "Oleksandr", 
							Surname = "Shevchenko"
						},
					},
					Genres = new Genre[]
					{
						new() { Id = 2, Name = "Detective"}
					}
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
					PublisherId = 2,
					AuthorId = 4,
					Publisher = new()
					{
						Id = 2,
						Name = "Smoloskyp"
					},
					Author = new()
					{
						Id = 4,
						Person = new()
						{
							Id = 6,
							Name = "Olha",
							Surname = "Syrenko"
						},
					},
					Genres = new Genre[]
					{
						new() { Id = 5, Name = "Fantasy"}
					}
				}
			};
	}
}
