using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Tests.Integration.Seeds;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Fixtures
{
	public class OrderSupabeseFixture : IAsyncLifetime
	{
		public Client client { get; private set; }

		public async Task DisposeAsync() =>
			await IntegrationTestHelper.ClearDb();

		public async Task InitializeAsync()
		{
			client = IntegrationTestHelper.TestClientInit();
			await IntegrationTestHelper.AddRecordToDb(OrderSeeder.Seed);
		}
	}
}
