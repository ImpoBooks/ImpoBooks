using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.Infrastructure.Errors.Cart;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ImpoBooks.BusinessLogic.Services.Cart
{
	public class CartService(IBookRepository bookRepository,
		IOrderRepository orderRepository) : ICartService
	{
		private readonly IBookRepository _bookRepository = bookRepository;
		private readonly IOrderRepository _orderRepository = orderRepository;

		public async Task<ErrorOr<Success>> CheckAvailabilityAsync(IEnumerable<OrderProductModel> products)
		{
			if(products.IsNullOrEmpty())
				return CartErrors.OrderProductsIsNull;

			// here could be quantity check
			foreach (var product in products)
			{
				Book DbBook = await _bookRepository.GetByIdAsync(product.Id);
				if (DbBook is null)
					return CartErrors.ProductNotFound;
			}

			return Result.Success;
		}

		public async Task<ErrorOr<Success>> SendToPaymentServiceAsync(CustomerRequisitesModel requisites)
		{
			if (requisites is null)
				return CartErrors.RequisitesIsNull;

			// here could be normal implementation
			await Task.Run(() => Thread.Sleep(100));

			return Result.Success;
		}

		public async Task<ErrorOr<Order>> SaveOrderAsync(OrderModel orderModel)
		{
			if (orderModel is null)
				return CartErrors.OrderModelIsNull;

			int orderCode = GenerateOrderCode(orderModel);
			Order dbOrder = await _orderRepository.GetByOrderCodeAsync(orderCode);
			if (dbOrder is not null)
				return CartErrors.OrderCodeAlreadyExists;

			Order newOrder = await CreateNewOrder(orderModel, orderCode);
			await _orderRepository.CreateAsync(newOrder);

			dbOrder = await _orderRepository.GetByOrderCodeAsync(orderCode);
			if (dbOrder is null)
				return CartErrors.OrderNotFound;

			return dbOrder.ToErrorOr()!;
		}

		private async Task<Order> CreateNewOrder(OrderModel orderModel, int orderCode)
		{
			return new Order() 
			{
				OrderCode = orderCode,
				CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				FirstName = orderModel.FirstName,
				LastName = orderModel.LastName,
				Email = orderModel.Email,
				Address = orderModel.Address,
				City = orderModel.City,
				ZipCode = orderModel.ZipCode,
				Country = orderModel.Country,
				TotalSum = orderModel.TotalSum,
				ExpensesList = await CreateExpenssesList(orderModel.Products)
			};
		}

		public int GenerateOrderCode(OrderModel orderModel)
		{
			var serializedObject = JsonConvert.SerializeObject(orderModel);

			using (var sha256 = SHA256.Create())
			{
				var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(serializedObject));
				return BitConverter.ToInt32(hashBytes);
			}
		}

		private async Task<string> CreateExpenssesList(IEnumerable<OrderProductModel> productsOrderInfo)
		{
			StringBuilder sb = new();

			foreach (var product in productsOrderInfo)
			{
				Book dbbook = await _bookRepository.GetByIdAsync(product.Id);
				sb.AppendLine($"{dbbook.Name} x{product.Count}: {dbbook.Price * product.Count}");
			}

			return sb.ToString();
		}
	}
}
