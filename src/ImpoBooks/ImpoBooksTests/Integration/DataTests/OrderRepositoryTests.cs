using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Tests.Integration.Fixtures;
using ImpoBooks.Tests.Integration.Seeds;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.DataTests
{
	[Collection("Integration Tests Collection")]
	public class OrderRepositoryTests : IClassFixture<OrderSupabeseFixture>
	{
		private readonly OrderRepository _repository;

		public OrderRepositoryTests(OrderSupabeseFixture fixture)
		{
			_repository = new(fixture.client);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		public async Task GetByIdAsync_ReturnExpectedOrder(int Id)
		{
			//Arrange
			Order expected = OrderSeeder.PreparedOrders.FirstOrDefault(x => x.Id == Id)!;

			//Act
			Order order = await _repository.GetByIdAsync(expected.Id);

			//Assert
			Assert.Equal(expected, order);
		}

		[Theory]
		[InlineData(1001)]
		[InlineData(1002)]
		[InlineData(1003)]
		public async Task GetByOrderCodeAsync_ReturnExpectedOrder(int code)
		{
			//Arrange
			Order expected = OrderSeeder.PreparedOrders.FirstOrDefault(x => x.OrderCode == code)!;

			//Act
			Order order = await _repository.GetByOrderCodeAsync(code);

			//Assert
			Assert.Equal(expected, order);
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedOrdersAmount()
		{
			//Arrange
			int expected = OrderSeeder.PreparedOrders.Count();

			//Act
			IEnumerable<Order> orders = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, orders.Count());
		}

		[Fact]
		public async Task GetAllAsync_ReturnExpectedOrdersContent()
		{
			Thread.Sleep(500);

			//Arrange
			IEnumerable<Order> expected = OrderSeeder.PreparedOrders;

			//Act
			IEnumerable<Order> orders = await _repository.GetAllAsync();

			//Assert
			Assert.Equal(expected, orders);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		public async Task CreateAsync_AddNewOrderToDb(int caseId)
		{
			//Arrange
			Order expected = NewOrders.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.CreateAsync(expected);
			Order actualOrder = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Equal(expected, actualOrder);

			await IntegrationTestHelper.RefreshDb(OrderSeeder.Seed);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public async Task DeleteAsync_RemoveOrderFromDb(int caseId)
		{
			//Arrange
			Order order = OrderSeeder.PreparedOrders.FirstOrDefault(x => x.Id == caseId)!;

			//Act
			await _repository.DeleteAsync(order);
			Order actualOrder = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualOrder);

			await IntegrationTestHelper.RefreshDb(OrderSeeder.Seed);
		}

		[Theory]
		[InlineData(4)]
		[InlineData(3)]
		[InlineData(2)]
		public async Task DeleteByIdAsync_RemoveOrderFromDb(int caseId)
		{
			//Arrange

			//Act
			await _repository.DeleteByIdAsync(caseId);
			Order actualOrder = await _repository.GetByIdAsync(caseId);

			//Assert
			Assert.Null(actualOrder);

			await IntegrationTestHelper.RefreshDb(OrderSeeder.Seed);
		}

		private IEnumerable<Order> NewOrders =>
			new Order[] 
			{
				new()
				{
					Id = 5,
					OrderCode = 1005,
					CreatedAt = "2024-12-01 14:30:00",
					FirstName = "Emma",
					LastName = "Brown",
					Email = "emma.brown@example.com",
					Address = "123 Maple Street",
					City = "Central City",
					ZipCode = "45678",
					Country = "USA",
					TotalSum = 65.50m,
					ExpensesList = "ProductM x1: 25.00, ProductN x2: 40.50"
				},
				new()
				{
					Id = 6,
					OrderCode = 1006,
					CreatedAt = "2024-12-01 15:00:00",
					FirstName = "Liam",
					LastName = "Davis",
					Email = "liam.davis@example.com",
					Address = "789 Walnut Avenue",
					City = "Coast City",
					ZipCode = "12321",
					Country = "Canada",
					TotalSum = 42.00m,
					ExpensesList = "ProductO x3: 42.00"
				},
				new()
				{
					Id = 7,
					OrderCode = 1007,
					CreatedAt = "2024-12-01 15:30:00",
					FirstName = "Olivia",
					LastName = "Wilson",
					Email = "olivia.wilson@example.com",
					Address = "456 Cedar Drive",
					City = "Ivy Town",
					ZipCode = "78901",
					Country = "UK",
					TotalSum = 200.75m,
					ExpensesList = "ProductP x2: 90.00, ProductQ x3: 30.25"
				}
			};
	}
}
