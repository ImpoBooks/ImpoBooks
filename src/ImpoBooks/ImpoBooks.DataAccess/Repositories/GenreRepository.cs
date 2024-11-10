using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using Supabase;
using Supabase.Postgrest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Repositories
{
	public class GenreRepository(Client client) : Repository<Genre>(client), IGenreRepository
	{
		private readonly Client _client = client;

		public async Task<Genre> GetByNameAsync(string name)
		{
			ModeledResponse<Genre> response = await _client.From<Genre>().Where(x => x.Name == name).Get();
			Genre genre = response.Model;
			return genre;
		}
	}
}
