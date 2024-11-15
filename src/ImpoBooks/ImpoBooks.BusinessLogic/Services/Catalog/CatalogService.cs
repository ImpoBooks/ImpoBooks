using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Extensions;
using ImpoBooks.BusinessLogic.Services.Mapping;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Catalog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImpoBooks.BusinessLogic.Services.Catalog
{
	public class CatalogService(IBookRepository bookRepository,
		IPersonRepository personRepository,
		IAuthorRepository authorRepository,
		IPublisherRepository publisherRepository,
		IGenreRepository genreRepository,
		IBookGenreRepository bookGenreRepository) : ICatalogService
	{
		private readonly IBookRepository _bookRepository = bookRepository;
		private readonly IPersonRepository _personReepository = personRepository;
		private readonly IAuthorRepository _authorRepository = authorRepository;
		private readonly IPublisherRepository _publisherRepository = publisherRepository;
		private readonly IGenreRepository _genreRepository = genreRepository;
		private readonly IBookGenreRepository _bookGenreRepository = bookGenreRepository;

		public async Task<ErrorOr<IEnumerable<CatalogBookModel>>> GetBooksAsync(FilterModel filterOptions)
		{
			if (filterOptions is null)
				return CatalogErrors.FilterIsNull;

			IEnumerable<Book> dbBooks = await _bookRepository.GetAllAsync();

			if (dbBooks is null || !dbBooks.Any())
				return CatalogErrors.BooksNotFound;

			IEnumerable<Book> filteredBooks = dbBooks.FilterByKeyWord(filterOptions.KeyWords)
				.FilterByGenre(filterOptions.Genre)
				.FilterByAuthor(filterOptions.Author)
				.FilterByPrice(filterOptions.MinPrice, filterOptions.MaxPrice)
				.FilterByRating(filterOptions.MinRating, filterOptions.MaxRating);

			if (filteredBooks is null || !filteredBooks.Any())
				return CatalogErrors.NoBookMatches;

			IEnumerable<CatalogBookModel> result = filteredBooks.Select(b => b.ToCatalogBookModel());

			return result.ToErrorOr();
		}

		public async Task<ErrorOr<CatalogBookModel>> CreateBookAsync(BookModel book)
		{
			if (book is null)
				return CatalogErrors.BookIsNull;

			Book dbBook = await _bookRepository.GetByNameAsync(book.Name);
			if (dbBook is not null)
				return CatalogErrors.AlreadyExists;

			string authorName = book.Author.Split(' ').First();
			string authorSurname = book.Author.Split(' ').Last();

			Author author = await _authorRepository.GetByFullNameAsync(authorName, authorSurname);
			author = await author.CreateIfNotExistAsync(_authorRepository, _personReepository, authorName, authorSurname);

			Publisher publisher = await _publisherRepository.GetByNameAsync(book.Publisher);
			publisher = await publisher.CreateIfNotExistAsync(_publisherRepository, book.Publisher);

			await _bookRepository.CreateAsync(new Book
			{
				Name = book.Name,
				PublisherId = publisher.Id,
				AuthorId = author.Id,
				Description = book.Description,
				ReleaseDate = book.ReleaseDate,
				Price = book.Price,
				Rating = book.Rating,
				Format = "Electronic"
			});

			dbBook = await _bookRepository.GetByNameAsync(book.Name);
			if (dbBook is null)
				return CatalogErrors.BookNotFound;

			IEnumerable<string> genres = book.Genres.Split(" ");
			foreach (string genre in genres)
			{
				Genre dbGenre = await _genreRepository.GetByNameAsync(genre);
				dbGenre = await dbGenre.CreateIfNotExistAsync(_genreRepository, genre);

				await _bookGenreRepository.CreateAsync(new BookGenre()
				{
					BookId = dbBook.Id,
					GenreId = dbGenre.Id
				});
			}

			dbBook = await _bookRepository.GetByNameAsync(book.Name);
			if (dbBook is null)
				return CatalogErrors.BookNotFound;

			CatalogBookModel result = dbBook.ToCatalogBookModel();

			return result.ToErrorOr();
		}

	}
}
