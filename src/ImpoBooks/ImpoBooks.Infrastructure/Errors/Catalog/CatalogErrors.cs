using ErrorOr;
using ImpoBooks.BusinessLogic.Errors;

namespace ImpoBooks.Infrastructure.Errors.Catalog
{
	public static class CatalogErrors
	{
		public static Error FilterIsNull =>
			Error.Custom(ErrorTypes.IsNull, "FilterOptions.IsNull", "Filter object was null.");
		public static Error BookIsNull =>
			Error.Custom(ErrorTypes.IsNull, "Book.IsNull", "Book object was null.");
		public static Error BookIdIsZero =>
			Error.Custom(ErrorTypes.WrongInfo, "Book.Id.WrongInfo", "Book id was equal to zero.");
		public static Error BooksNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Books.NotFound", "Failed to get books.");
		public static Error GenresNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Genres.NotFound", "Failed to get genres.");
		public static Error BookNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Book.NotFound", "Failed to get book.");
		public static Error NoBookMatches =>
			Error.Custom(ErrorTypes.NotFound, "Books.NoBookMatches", "No books were found for the established criterias");
		public static Error AlreadyExists =>
			Error.Custom(ErrorTypes.NotFound, "Book.AlreadyExists", "Book with this name already exists.");
	}
}
