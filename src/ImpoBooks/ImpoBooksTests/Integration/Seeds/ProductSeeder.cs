using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class ProductSeeder
	{
		public static IEnumerable<Product> PreparedProducts =>
			new Product[]
			{
				new() {Id = 1, BookId = 1},
				new() {Id = 2, BookId = 2},
				new() {Id = 3, BookId = 3},
				new() {Id = 4, BookId = 4},
				new() {Id = 5, BookId = 5},
			};

		public static string Seed =
			"INSERT INTO \"Products\" (id, book_id) VALUES" +
			string.Join(", ", PreparedProducts.Select(g => $"({g.Id}, {g.BookId})")) +
			";";
	}
}
