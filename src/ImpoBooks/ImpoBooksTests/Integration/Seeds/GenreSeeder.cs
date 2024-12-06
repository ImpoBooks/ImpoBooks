using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class GenreSeeder
	{
		public static IEnumerable<Genre> PreparedGenres =>
			new Genre[]
			{
				new() { Id = 1, Name = "Science-Fiction"},
				new() { Id = 2, Name = "Detective"},
				new() { Id = 3, Name = "Detective"},
				new() { Id = 4, Name = "Adventure"},
				new() { Id = 5, Name = "Fantasy"}
			};

		public static string Seed =
			"INSERT INTO \"Genres\" (id, name) VALUES" +
			string.Join(", ", PreparedGenres.Select(g => $"({g.Id}, '{g.Name}')")) +
			";";
	}
}
