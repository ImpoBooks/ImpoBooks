using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class AuthorSeeder
	{
		public static IEnumerable<Author> PreparedAuthors =>
			new Author[]
			{
				new() { Id = 1, PersonId = 4},
				new() { Id = 2, PersonId = 3},
				new() { Id = 3, PersonId = 2},
				new() { Id = 4, PersonId = 6},
				new() { Id = 5, PersonId = 1}
			};

		public static string Seed =
			"INSERT INTO \"Authors\" (id, person_id) VALUES" +
			string.Join(", ", PreparedAuthors.Select(a => $"({a.Id}, {a.PersonId})")) +
			";";
	}
}
