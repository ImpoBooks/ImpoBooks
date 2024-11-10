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
	public class AuthorRepository(Client client) : Repository<Author>(client), IAuthorRepository
	{
		private readonly Client _client = client;

		public async Task<Author> GetByFullNameAsync(string name, string surname)
		{
			ModeledResponse<Author> response = await _client.From<Author>()
				.Select("*, Persons!inner(*))")
				.Filter("Persons.name", Operator.Equals, name)
				.Filter("Persons.surname", Operator.Equals, surname)
				.Get();

			Author author = response.Model;
			return author;
		}
	}
}
