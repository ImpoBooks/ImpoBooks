using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class UserSeeder
	{
		public static IEnumerable<User> PreparedUsers =>
			new User[]
			{
				new() {Id = 1, Name = "Mikhail"},
				new() {Id = 2, Name = "Bender"},
				new() {Id = 3, Name = "Kyle"},
				new() {Id = 4, Name = "Nikita"},
			};

		public static string Seed =
			"INSERT INTO \"Users\" (id, name) VALUES" +
			string.Join(", ", PreparedUsers.Select(g => $"({g.Id}, '{g.Name}')")) +
			";";
	}
}
