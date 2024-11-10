using ImpoBooks.DataAccess.Entities;
using ImpoBooksTests;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.DataTests.Fixtures
{
    public class PublisherSupabaseFixture : IAsyncLifetime
    {
        public Client client { get; private set; }
        public IEnumerable<Publisher> PrepearedPublishers =>
            new Publisher[]
	        {
	            new() { Id = 1, Name = "Ranok"},
	            new() { Id = 2, Name = "Smoloskyp"},
	            new() { Id = 3, Name = "Old Lion Publishing House"},
	            new() { Id = 4, Name = "Nash Format"},
	            new() { Id = 5, Name = "Vivat"}
            };

		public async Task DisposeAsync()
        {
            await IntegrationTestHelper.ClearTable<Publisher>(client);
        }

        public async Task InitializeAsync()
        {
            client = IntegrationTestHelper.TestClientInit();
            await IntegrationTestHelper.InitTable(client, PrepearedPublishers);
        }

    }
}
