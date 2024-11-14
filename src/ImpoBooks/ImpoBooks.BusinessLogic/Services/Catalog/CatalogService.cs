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

namespace ImpoBooks.BusinessLogic.Services.Catalog
{
	public class CatalogService(IBookRepository bookRepository) : ICatalogService
	{
		private readonly IBookRepository _bookRepository = bookRepository;
		public async Task<ErrorOr<IEnumerable<CatalogBookModel>>> GetBooksAsync(FilterModel filterOptions)
		{
			if (filterOptions is null)
				return CatalogErrors.IsNull;

			IEnumerable<Book> dbBooks = await _bookRepository.GetAllAsync();

			if (dbBooks is null)
				return CatalogErrors.BooksNotFound;

			IEnumerable<Book> filteredBooks = dbBooks.FilterByKeyWord(filterOptions.KeyWords)
				.FilterByGenre(filterOptions.Genre)
				.FilterByAuthor(filterOptions.Author)
				.FilterByPrice(filterOptions.MinPrice, filterOptions.MaxPrice)
				.FilterByRating(filterOptions.MinRating, filterOptions.MaxRating);

			if (filteredBooks is null)
				return CatalogErrors.NoBookMatches;

			IEnumerable<CatalogBookModel> result = filteredBooks.Select(b => b.ToCatalogBookModel());

			return result.ToErrorOr();
		}

		
	}
}
