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
    public class PersonSupabaseFixture : IAsyncLifetime
    {
        public Client client { get; private set; }
        public IEnumerable<Person> PreparedPersons =>
            new Person[]
            {
                new() { Id = 1, Name = "Oleksandr", Surname = "Shevchenko"},
                new() { Id = 2, Name = "Dmytro", Surname = "Kovalchuk"},
                new() { Id = 3, Name = "Andriy", Surname = "Grytsenko"},
                new() { Id = 4, Name = "Volodymyr", Surname = "Tkachenko"},
                new() { Id = 5, Name = "Kateryna", Surname = "Moroz"},
                new() { Id = 6, Name = "Olha", Surname = "Sydenko"},
                new() { Id = 7, Name = "Iryna", Surname = "Petrenko"},
                new() { Id = 8, Name = "Joe", Surname = "Biden"},
                new() { Id = 9, Name = "Fedir", Surname = "Denchyk"},
                new() { Id = 10, Name = "Tyler", Surname = "Durden"}
			};

        public async Task DisposeAsync() =>
            await IntegrationTestHelper.ClearTable<Person>(client);

        public async Task InitializeAsync()
        {
            client = IntegrationTestHelper.TestClientInit();
            await IntegrationTestHelper.InitTable(client, PreparedPersons);
        }

    }
}
