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
				new() {Id = 1, Name = "Mikhail", RoleId = 2},
				new() {Id = 2, Name = "Bender", RoleId = 2},
				new() {Id = 3, Name = "Kyle", RoleId = 2},
				new() {Id = 4, Name = "Nikita", RoleId = 2},
			};

		public static string Seed =
			"INSERT INTO \"Users\" (id, name, role_id) VALUES" +
			string.Join(", ", PreparedUsers.Select(u => $"({u.Id}, '{u.Name}', {u.RoleId})")) +
			";";
	}
}
