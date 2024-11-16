using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;

namespace ImpoBooks.DataAccess.Repositories
{
	public class BookGenreRepository(Client client) : Repository<BookGenre, AutoIncBookGenre>(client), IBookGenreRepository
	{
	}
}
