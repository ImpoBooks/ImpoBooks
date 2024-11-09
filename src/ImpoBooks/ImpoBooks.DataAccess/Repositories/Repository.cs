using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using Supabase;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Repositories
{
	public class Repository<T>(Client client) : IRepository<T> where T : BaseModelExtended, new()
	{
		private readonly Client _client = client;
		public async Task CreateAsync(T entity) =>
			await _client.From<T>().Insert(entity);
	
		public async Task<T> GetByIdAsync(int id)
		{
			ModeledResponse<T> response = await _client.From<T>().Where(x => x.Id == id).Get();
			T entity = response.Model;
			return entity;
		}
		public async Task<IEnumerable<T>> GetAllAsync()
		{
			ModeledResponse<T> response = await _client.From<T>().Get();
			IEnumerable<T> entity = response.Models;
			return entity;
		}
		public async Task DeleteAsync(T entity) =>
			await _client.From<T>().Where(x => x.Id == entity.Id).Delete();

		public async Task DeleteByIdAsync(int id) =>
			await _client.From<T>().Where(x => x.Id == id).Delete();

		public async Task UpdateAsync(T entity) =>
			await _client.From<T>().Where(x => x.Id == entity.Id).Update(entity);
	}
}
