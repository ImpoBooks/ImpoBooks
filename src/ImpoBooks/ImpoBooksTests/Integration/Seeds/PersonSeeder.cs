using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class PersonSeeder
	{
		public static IEnumerable<Person> PreparedPersons =>
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

		public static string Seed =
			"INSERT INTO \"Persons\" (id, name, surname) VALUES" +
			string.Join(", ", PreparedPersons.Select(p => $"({p.Id}, '{p.Name}', '{p.Surname}')")) +
			";";
	}
}
