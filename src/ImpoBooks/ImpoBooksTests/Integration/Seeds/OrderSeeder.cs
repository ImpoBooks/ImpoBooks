using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class OrderSeeder
	{
		public static IEnumerable<Order> PreparedOrders =>
			new Order[]
			{
				new()
				{
					Id = 1,
					OrderCode = 1001,
					CreatedAt = "2024-12-01 12:30:00",
					FirstName = "John",
					LastName = "Doe",
					Email = "john.doe@example.com",
					Address = "123 Elm Street",
					City = "Springfield",
					ZipCode = "12345",
					Country = "USA",
					TotalSum = 107.96m,
					ExpensesList = "ProductA x2: 39.00, ProductB x1: 15.99, ProductC x3: 52.97"
				},
				new()
				{
					Id= 2,
					OrderCode = 1002,
					CreatedAt = "2024-12-01 13:00:00",
					FirstName = "Jane",
					LastName = "Smith",
					Email = "jane.smith@example.com",
					Address = "456 Oak Avenue",
					City = "Metropolis",
					ZipCode = "54321",
					Country = "Canada",
					TotalSum = 34.99m,
					ExpensesList = "ProductD x1: 22.99, ProductE x1: 12.00"
				},
				new()
				{
					Id = 3,
					OrderCode = 1003,
					CreatedAt = "2024-12-01 13:30:00",
					FirstName = "Alice",
					LastName = "Johnson",
					Email = "alice.johnson@example.com",
					Address = "789 Pine Road",
					City = "Gotham",
					ZipCode = "67890",
					Country = "UK",
					TotalSum = 89.94m,
					ExpensesList = "ProductF x2: 35.98, ProductG x1: 17.99, ProductH x2: 25.98, ProductI x1: 9.99"
				},
				new()
				{
					Id = 4,
					OrderCode = 1004,
					CreatedAt = "2024-12-01 14:00:00",
					FirstName = "Bob",
					LastName = "Williams",
					Email = "bob.williams@example.com",
					Address = "321 Birch Lane",
					City = "Star City",
					ZipCode = "98765",
					Country = "Australia",
					TotalSum = 82.98m,
					ExpensesList = "ProductJ x1: 27.00, ProductK x1: 18.99, ProductL x2: 36.99"
				}
			};

		public static string Seed =
			"INSERT INTO \"Orders\" (id, created_at, order_code, first_name, last_name, email, address, city, zip_code, country, total_sum, products) VALUES" +
			string.Join(", ", PreparedOrders.Select(o => $"(" +
			$"{o.Id}," +
			$"'{o.CreatedAt}'," + 
			$"{o.OrderCode}," +
			$"'{o.FirstName}'," +
			$"'{o.LastName}'," +
			$"'{o.Email}'," +
			$"'{o.Address}'," +
			$"'{o.City}'," +
			$"'{o.ZipCode}'," +
			$"'{o.Country}'," +
			$"{o.TotalSum.ToString().Replace(",", ".")}," +
			$"'{o.ExpensesList}')")) +
			";";
	}
}
