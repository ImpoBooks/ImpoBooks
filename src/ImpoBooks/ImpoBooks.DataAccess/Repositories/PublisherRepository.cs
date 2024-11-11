using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using Supabase;
using Supabase.Postgrest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Supabase.Postgrest.Constants;

namespace ImpoBooks.DataAccess.Repositories
{
	public class PublisherRepository(Client client) : Repository<Publisher>(client), IPublisherRepository
	{
		private readonly Client _client = client;

		public async Task<Publisher> GetByNameAsync(string name)
		{
			ModeledResponse<Publisher> response = await _client.From<Publisher>().Where(x => x.Name == name).Get();
			Publisher publisher = response.Model;
			if (publisher is null) return publisher;

			ModeledResponse<Book> responseB = await _client.From<Book>().Where(x => x.PublisherId == publisher.Id).Get();
			publisher.Books = responseB.Models;

			return publisher;
		}

		public override async Task<Publisher> GetByIdAsync(int id)
		{
			ModeledResponse<Publisher> responseP = await _client.From<Publisher>().Where(x => x.Id == id).Get();
			Publisher publisher = responseP.Model;
			if (publisher is null) return publisher;

			ModeledResponse<Book> responseB = await _client.From<Book>().Where(x => x.PublisherId == publisher.Id).Get();
			publisher.Books = responseB.Models;

			return publisher;
		}

		public override async Task<IEnumerable<Publisher>> GetAllAsync()
		{
			ModeledResponse<Publisher> response = await _client.From<Publisher>().Get();
			IEnumerable<Publisher> publishers = response.Models;

			foreach (Publisher publisher in publishers) 
			{
				ModeledResponse<Book> responseB = await _client.From<Book>().Where(x => x.PublisherId == publisher.Id).Get();
				publisher.Books = responseB.Models;
			}

			return publishers;
		}

	}
}
