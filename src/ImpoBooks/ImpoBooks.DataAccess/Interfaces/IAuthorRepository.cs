using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Interfaces
{
	public interface IAuthorRepository : IRepository<Author>
	{
		Task<Author> GetByFullNameAsync(string name, string surname);
	}
}
