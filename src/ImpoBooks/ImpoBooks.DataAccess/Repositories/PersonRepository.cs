using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Entities.AutoIncremented;
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
	public class PersonRepository(Client client) : Repository<Person, AutoIncPerson>(client), IPersonRepository
	{
		private readonly Client _client = client;

		public async Task<IEnumerable<Person>> GetByFullNameAsync(string name, string surname)
		{
			ModeledResponse<Person> response = await _client.From<Person>().Where(x => x.Name == name && x.Surname == surname).Get();
			IEnumerable<Person> person = response.Models;
			return person;
		}
	}
}
