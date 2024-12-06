using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Extensions;
using ImpoBooks.BusinessLogic.Services.Mapping;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Catalog;
using ImpoBooks.Infrastructure.Errors.Product;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
		IBookGenreRepository bookGenreRepository,
		IProductRepository productRepository) : ICatalogService
	{
		private readonly IBookRepository _bookRepository = bookRepository;
		private readonly IPersonRepository _personReepository = personRepository;
		private readonly IAuthorRepository _authorRepository = authorRepository;
		private readonly IPublisherRepository _publisherRepository = publisherRepository;
		private readonly IGenreRepository _genreRepository = genreRepository;
		private readonly IBookGenreRepository _bookGenreRepository = bookGenreRepository;
		private readonly IProductRepository _productRepository = productRepository;


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

			Author author = await GetOrCreateAuthorAsync(book);
			Publisher publisher = await GetOrCreatePublisherAsync(book);

			await _bookRepository.CreateAsync(new Book
			{
				Name = book.Name,
				PublisherId = publisher.Id,
				AuthorId = author.Id,
				Description = book.Description,
				ReleaseDate = book.ReleaseDate,
				Price = book.Price,
				Rating = book.Rating,
				Format = "Electronic",
				ImageUrl = book.ImageUrl
			});

			dbBook = await _bookRepository.GetByNameAsync(book.Name);
			if (dbBook is null)
				return CatalogErrors.BookNotFound;

			await AddGenresToBookAsync(dbBook.Id, book);

			dbBook = await _bookRepository.GetByNameAsync(book.Name);
			if (dbBook is null)
				return CatalogErrors.BookNotFound;

			DataAccess.Entities.Product createdProduct = await CreateBookProduct(dbBook);
			if (createdProduct is null)
				return ProductErrors.ProductNotFound;

			CatalogBookModel result = dbBook.ToCatalogBookModel();

			return result.ToErrorOr();
		}

		public async Task<ErrorOr<CatalogBookModel>> UpdateBookAsync(int bookId, BookModel book)
		{
			if (book is null)
				return CatalogErrors.BookIsNull;

			Book dbBook = await _bookRepository.GetByIdAsync(bookId);
			if (dbBook is null)
				return CatalogErrors.BookNotFound;

			Author author = await GetOrCreateAuthorAsync(book);
			Publisher publisher = await GetOrCreatePublisherAsync(book);

			Book updatedBook = UpdateBookFields(dbBook, book, author, publisher);
			await _bookRepository.UpdateAsync(updatedBook);

			await RemoveBookGenresRelations(bookId);
			await AddGenresToBookAsync(bookId, book);

			updatedBook = await _bookRepository.GetByIdAsync(bookId);
			if (updatedBook is null)
				return CatalogErrors.BookNotFound;

			CatalogBookModel result = updatedBook.ToCatalogBookModel();
			return result.ToErrorOr();
		}

		public async Task<ErrorOr<Success>> DeleteBookAsync(int bookId)
		{
			if (bookId is 0)
				return CatalogErrors.BookIdIsZero;

			Book dbBook = await _bookRepository.GetByIdAsync(bookId);
			if (dbBook is null)
				return CatalogErrors.BookNotFound;

			await _bookRepository.DeleteByIdAsync(bookId);
			return Result.Success;
		}

		public async Task<ErrorOr<IEnumerable<GenreModel>>> GetGenresAsync()
		{
			IEnumerable<Genre> dbGenres = await _genreRepository.GetAllAsync();
			if (dbGenres.IsNullOrEmpty())
				return CatalogErrors.GenresNotFound;

			IEnumerable<GenreModel> result = dbGenres.Select(g => g.ToGenreModel());
			return result.ToErrorOr();
		}

		public async Task<ErrorOr<IEnumerable<AuthorModel>>> GetAuthorsAsync()
		{
			IEnumerable<Author> dbAuthors = await _authorRepository.GetAllAsync();
			if (dbAuthors.IsNullOrEmpty())
				return CatalogErrors.AuthorsNotFound;

			IEnumerable<AuthorModel> result = dbAuthors.Select(g => g.ToAuthorModel());
			return result.ToErrorOr();
		}

		private async Task<Author> GetOrCreateAuthorAsync(BookModel bookModel)
		{
			string authorName = bookModel.Author.Split(' ').First();
			string authorSurname = bookModel.Author.Split(' ').Last();

			Author author = await authorRepository.GetByFullNameAsync(authorName, authorSurname);
			return await author.CreateIfNotExistAsync(_authorRepository, _personReepository, authorName, authorSurname);
		}

		private async Task<Publisher> GetOrCreatePublisherAsync(BookModel bookModel)
		{
			Publisher publisher = await _publisherRepository.GetByNameAsync(bookModel.Publisher);
			return await publisher.CreateIfNotExistAsync(_publisherRepository, bookModel.Publisher);
		}


		private Book UpdateBookFields(Book target, BookModel bookModel, Author author, Publisher publisher)
		{
			if (!bookModel.Name.IsNullOrEmpty()) target.Name = bookModel.Name;
			if (!bookModel.Description.IsNullOrEmpty()) target.Description = bookModel.Description;
			if (!bookModel.ReleaseDate.IsNullOrEmpty()) target.ReleaseDate = bookModel.ReleaseDate;
			if (!bookModel.ImageUrl.IsNullOrEmpty()) target.ImageUrl = bookModel.ImageUrl;
			if (bookModel.Price != 0) target.Price = bookModel.Price;
			if (bookModel.Rating != 0) target.Rating = bookModel.Rating;
			target.AuthorId = author.Id;
			target.PublisherId = publisher.Id;
			target.Genres = null;

			return target;
		}

		private async Task RemoveBookGenresRelations(int bookId)
		{
			IEnumerable<BookGenre> relations = await _bookGenreRepository.GetAllAsync();
			IEnumerable<BookGenre> currentBookRelations = relations.Where(r => r.BookId == bookId);

			foreach (BookGenre relation in currentBookRelations)
			{
				await _bookGenreRepository.DeleteAsync(relation);
			}
		}

		private async Task AddGenresToBookAsync(int bookId, BookModel bookModel)
		{
			IEnumerable<string> genres = bookModel.Genres.Split(" ");

			foreach (string genre in genres)
			{
				Genre dbGenre = await _genreRepository.GetByNameAsync(genre);
				dbGenre = await dbGenre.CreateIfNotExistAsync(_genreRepository, genre);

				await _bookGenreRepository.CreateAsync(new BookGenre()
				{
					BookId = bookId,
					GenreId = dbGenre.Id
				});
			}
		}

		private async Task<DataAccess.Entities.Product> CreateBookProduct(Book book)
		{
			await _productRepository.CreateAsync
			(
				new DataAccess.Entities.Product() 
				{ 
					Id = book.Id,
					BookId = book.Id
				}
			);

			return await _productRepository.GetByNameAsync(book.Name);
		}
	}
}
