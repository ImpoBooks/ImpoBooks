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
    public class AuthorSupabaseFixture : IAsyncLifetime
    {
        public Client client { get; private set; }
        public IEnumerable<Author> PrepearedAuthors =>
            new Author[]
            {
                        new() { Id = 1, PersonId = 4},
                        new() { Id = 2, PersonId = 3},
                        new() { Id = 3, PersonId = 2},
                        new() { Id = 4, PersonId = 6},
                        new() { Id = 5, PersonId = 1}
            };
        public IEnumerable<Person> PrepearedPersons =>
            new Person[]
            {
                new() { Id = 4, Name = "Volodymyr", Surname = "Tkachenko"},
                new() { Id = 3, Name = "Andriy", Surname = "Grytsenko"},
                new() { Id = 2, Name = "Dmytro", Surname = "Kovalchuk"},
                new() { Id = 6, Name = "Olha", Surname = "Syrenko"},
                new() { Id = 1, Name = "Oleksandr", Surname = "Shevchenko"}

			};

        public async Task DisposeAsync()
        {
            await IntegrationTestHelper.ClearTable<Author>(client);
            await IntegrationTestHelper.ClearTable<Person>(client);

        }

        public async Task InitializeAsync()
        {
            client = IntegrationTestHelper.TestClientInit();
            await IntegrationTestHelper.InitTable(client, PrepearedPersons);
            await IntegrationTestHelper.InitTable(client, PrepearedAuthors);
        }

    }
}
