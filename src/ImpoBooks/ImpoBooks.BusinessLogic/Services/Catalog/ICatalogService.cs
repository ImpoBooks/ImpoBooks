using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Catalog
{
	public interface ICatalogService
	{
		Task<ErrorOr<IEnumerable<CatalogBookModel>>> GetBooksAsync(FilterModel filterOptions);
	}
}
