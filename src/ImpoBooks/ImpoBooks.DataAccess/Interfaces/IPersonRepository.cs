using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Interfaces
{
	internal interface IPersonRepository : IRepository<Person>
	{
		Task<IEnumerable<Person>> GetByFullNameAsync(string name, string surname);
	}
}
