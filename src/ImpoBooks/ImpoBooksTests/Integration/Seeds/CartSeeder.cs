using ImpoBooks.BusinessLogic.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class CartSeeder
	{
		public static List<OrderModel> PreparedOrderModels =>
			new List<OrderModel>
			{
				new ()
				{
					FirstName = "John",
					LastName = "Doe",
					Email = "john.doe@example.com",
					Address = "123 Elm Street",
					City = "Springfield",
					ZipCode = "12345",
					Country = "USA",
					TotalSum = 100.97m,
					Products = new OrderProductModel[]
					{
						new () {Id =  1, Count = 2},
						new () {Id =  3, Count = 1},
					}
				},
				new()
				{
					FirstName = "Jane",
					LastName = "Smith",
					Email = "jane.smith@example.com",
					Address = "456 Oak Avenue",
					City = "Metropolis",
					ZipCode = "54321",
					Country = "Canada",
					TotalSum = 162.95m,
					Products = new OrderProductModel[]
					{
						new() {Id =  1, Count = 1},
						new() {Id =  2, Count = 1},
						new() {Id =  3, Count = 3},
					}
				},
				new()
				{
					FirstName = "Alice",
					LastName = "Johnson",
					Email = "alice.johnson@example.com",
					Address = "789 Pine Road",
					City = "Gotham",
					ZipCode = "67890",
					Country = "UK",
					TotalSum = 199.90m,
					Products = new OrderProductModel[]
					{
						new() {Id =  2, Count = 10},
					}
				},
			};
	}
}
