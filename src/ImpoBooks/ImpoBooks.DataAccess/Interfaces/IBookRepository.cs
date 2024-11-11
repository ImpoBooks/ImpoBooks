using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Interfaces
{
	public interface IBookRepository : IRepository<Book>
	{
		Task<Book> GetByNameAsync(string name);
		Task<IEnumerable<Book>> GetByReleasDateAsync(DateTime releasDate);
		Task<IEnumerable<Book>> GetByPriceAsync(decimal price);
		Task<IEnumerable<Book>> GetByRaitingAsync(decimal raiting);
		Task<IEnumerable<Book>> GetByFormatAsync(string format);
		Task<IEnumerable<Book>> GetByPublisherNameAsync(string name);
		Task<IEnumerable<Book>> GetByAuthorFullNameAsync(string name, string surname);
	}
}
