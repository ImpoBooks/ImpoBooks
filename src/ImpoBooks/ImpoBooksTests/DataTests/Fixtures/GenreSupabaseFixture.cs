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
    public class GenreSupabaseFixture : IAsyncLifetime
    {
        public Client client { get; private set; }
        public IEnumerable<Genre> PrepearedGenres =>
            new Genre[]
	        {
                new() { Id = 1, Name = "Science Fiction"},
                new() { Id = 2, Name = "Detective"},
                new() { Id = 3, Name = "Detective"},
                new() { Id = 4, Name = "Adventure"},
                new() { Id = 5, Name = "Fantasy"}
			};

		public async Task DisposeAsync()
        {
            await IntegrationTestHelper.ClearTable<Genre>(client);
        }

        public async Task InitializeAsync()
        {
            client = IntegrationTestHelper.TestClientInit();
            await IntegrationTestHelper.InitTable(client, PrepearedGenres);
        }

    }
}
