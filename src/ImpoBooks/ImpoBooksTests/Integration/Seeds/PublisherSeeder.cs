using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class PublisherSeeder
	{
		public static IEnumerable<Publisher> PreparedPublishers =>
			new Publisher[]
			{
				new() { Id = 1, Name = "Ranok"},
				new() { Id = 2, Name = "Smoloskyp"},
				new() { Id = 3, Name = "Old Lion Publishing House"},
				new() { Id = 4, Name = "Nash Format"},
				new() { Id = 5, Name = "Vivat"}
			};

		public static string Seed =
			"INSERT INTO \"Publishers\" (id, name) VALUES" +
			string.Join(", ", PreparedPublishers.Select(p => $"({p.Id}, '{p.Name}')")) +
			";";
	}
}
