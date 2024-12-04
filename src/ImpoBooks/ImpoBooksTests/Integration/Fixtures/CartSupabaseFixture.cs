using ImpoBooks.Tests.Integration.Seeds;
using Supabase;

namespace ImpoBooks.Tests.Integration.Fixtures
{
	public class CartSupabaseFixture : IAsyncLifetime
	{
		public Client client { get; private set; }

		public async Task DisposeAsync() =>
			await IntegrationTestHelper.ClearDb();

		public async Task InitializeAsync()
		{
			client = IntegrationTestHelper.TestClientInit();
			await IntegrationTestHelper.AddRecordToDb
			(
				PersonSeeder.Seed +
				AuthorSeeder.Seed +
				PublisherSeeder.Seed +
				GenreSeeder.Seed +
				BookSeeder.Seed +
				BookGenreSeeder.Seed
			);
		}
	}
}
