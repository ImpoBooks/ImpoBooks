
using ErrorOr;
using ImpoBooks.BusinessLogic.Errors;
using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Infrastructure.Errors.Catalog
{
	public static class CatalogErrors
	{
		public static Error IsNull =>
			Error.Custom(ErrorTypes.IsNull, "FilterOptions.IsNull", "Filter object was null.");

		public static Error BooksNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Books.NotFound", "Failed to get books.");

		public static Error NoBookMatches =>
			Error.Custom(ErrorTypes.NotFound, "Books.NoBookMatches", "No books were found for the established criterias");
	}
}
