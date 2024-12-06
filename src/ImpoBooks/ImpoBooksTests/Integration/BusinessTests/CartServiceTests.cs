using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Cart;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Cart;
using ImpoBooks.Tests.Integration.Fixtures;
using ImpoBooks.Tests.Integration.Seeds;
using Supabase;

namespace ImpoBooks.Tests.Integration.BusinessTests
{
	[Collection("Integration Tests Collection")]
	public class CartServiceTests : IClassFixture<CartSupabaseFixture>
	{
		private readonly CartService _cartService;

		public CartServiceTests(CartSupabaseFixture fixture)
		{
			_cartService = new
				(
					new BookRepository(fixture.client),
					new OrderRepository(fixture.client)
				);
		}

		[Fact]
		public async Task CheckAvailabilityAsync_ReturnOrderProductsIsNullError()
		{
			//Arrang
			IEnumerable<OrderProductModel>? products = null;

			//Act
			ErrorOr<Success> result = await _cartService.CheckAvailabilityAsync(products);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CartErrors.OrderProductsIsNull, result.FirstError);
		}

		[Fact]
		public async Task CheckAvailabilityAsync_ReturnProductNotFoundError()
		{
			//Arrang
			IEnumerable<OrderProductModel> products = new OrderProductModel[]
			{
				new() { Id = 1, Count = 8 },
				new() { Id = 50, Count = 3 },
			};

			//Act
			ErrorOr<Success> result = await _cartService.CheckAvailabilityAsync(products);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CartErrors.ProductNotFound, result.FirstError);
		}

		[Fact]
		public async Task CheckAvailabilityAsync_ReturnExpectedResult()
		{
			//Arrang
			IEnumerable<OrderProductModel> products = new OrderProductModel[]
			{
				new() { Id = 1, Count = 1 },
				new() { Id = 2, Count = 1 },
				new() { Id = 3, Count = 1 },
			};

			//Act
			ErrorOr<Success> result = await _cartService.CheckAvailabilityAsync(products);

			//Assert
			Assert.Equal(Result.Success, result);
		}

		[Fact]
		public async Task SendToPaymentServiceAsync_ReturnRequisitesIsNullError()
		{
			//Arrang
			CustomerRequisitesModel? requisites = null;

			//Act
			ErrorOr<Success> result = await _cartService.SendToPaymentServiceAsync(requisites);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CartErrors.RequisitesIsNull, result.FirstError);
		}

		[Fact]
		public async Task SendToPaymentServiceAsync_ReturnExpectedResult()
		{
			//Arrang
			CustomerRequisitesModel requisites = new()
			{
				FirstName = "Jane",
				LastName = "Smith",
				Email = "jane.smith@example.com",
				CardNumber = "0123 4567 8910 1112",
				ExpirationDate = "12/42",
				CVV = "777",
				TotalSum = 162.95m,
			};

			//Act
			ErrorOr<Success> result = await _cartService.SendToPaymentServiceAsync(requisites);

			//Assert
			Assert.Equal(Result.Success, result);
		}

		[Fact]
		public async Task SaveOrderAsync_ReturnOrderModelIsNullError()
		{
			//Arrang
			OrderModel? orderModel = null;

			//Act
			ErrorOr<OrderCheckModel> result = await _cartService.SaveOrderAsync(orderModel);

			//Assert
			Assert.True(result.IsError);
			Assert.Equal(CartErrors.OrderModelIsNull, result.FirstError);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		public async Task SaveOrderAsync_ReturnExpectedResult(int caseId )
		{
			//Arrang
			OrderModel orderModel = CartSeeder.PreparedOrderModels[caseId];
			OrderCheckModel expected = ExpectedOrders[caseId];
			expected.OrderCode = _cartService.GenerateOrderCode(orderModel);

			//Act
			ErrorOr<OrderCheckModel> result = await _cartService.SaveOrderAsync(orderModel);

			//Assert
			Assert.Equal(expected, result.Value);
		}

		public List<OrderCheckModel> ExpectedOrders =>
			new List<OrderCheckModel>
			{
				new()
				{
					FirstName = "John",
					LastName = "Doe",
					Email = "john.doe@example.com",
					Address = "123 Elm Street",
					City = "Springfield",
					ZipCode = "12345",
					Country = "USA",
					TotalSum = 100.97m,
					ExpensesList = $"The Secret Garden x2: {51.98m}{Environment.NewLine}The Da Vinci Code x1: {38.99m}{Environment.NewLine}"
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
					ExpensesList = $"The Secret Garden x1: {25.99m}{Environment.NewLine}Murder on the Orient Express x1: {19.99m}{Environment.NewLine}The Da Vinci Code x3: {116.97m}{Environment.NewLine}"
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
					ExpensesList = $"Murder on the Orient Express x10: {199.90m}{Environment.NewLine}"
				}
			};
	}
}
