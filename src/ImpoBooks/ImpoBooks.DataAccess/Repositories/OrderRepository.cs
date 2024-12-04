using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using Supabase;
using Supabase.Postgrest.Responses;

namespace ImpoBooks.DataAccess.Repositories
{
	public class OrderRepository(Client client) : Repository<Order, AutoIncOrder>(client), IOrderRepository
	{
		private readonly Client _client = client;

		public async Task<Order> GetByOrderCodeAsync(int code)
		{
			ModeledResponse<Order> response = await _client.From<Order>()
				.Where(x => x.OrderCode == code)
				.Get();

			Order order = response.Model;
			return order;
		}
	}
}
