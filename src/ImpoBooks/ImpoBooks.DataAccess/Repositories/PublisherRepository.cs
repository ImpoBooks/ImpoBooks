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
	public class PublisherRepository(Client client) : Repository<Publisher>(client), IPublisherRepository
	{
		private readonly Client _client = client;

		public async Task<Publisher> GetByNameAsync(string name)
		{
			ModeledResponse<Publisher> response = await _client.From<Publisher>().Where(x => x.Name == name).Get();
			Publisher publisher = response.Model;
			return publisher;
		}
	}
}
