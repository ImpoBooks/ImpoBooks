using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Cart;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController(ICartService cartService) : Controller
	{
		private readonly ICartService _cartService = cartService;

		[HttpPost("order")]
		[ProducesResponseType<OrderCheckModel>(StatusCodes.Status201Created)]
		[ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
		public async Task<IResult> SaveOrder(CartOrderRequest orderRequest)
		{
			ErrorOr<Success> isAvailable =
				await _cartService.CheckAvailabilityAsync(orderRequest.ToOrderProductModelCollection());

			if (isAvailable != Result.Success)
			{
				return isAvailable.Match(
					_ => Results.Ok(),
					errors => Results.BadRequest(errors.First())
				);
			}

			ErrorOr<Success> isOrderPaid =
				await _cartService.SendToPaymentServiceAsync(orderRequest.ToRequisitesModel());

			if (isOrderPaid != Result.Success)
			{
				return isOrderPaid.Match(
					_ => Results.Ok(),
					errors => Results.BadRequest(errors.First())
				);
			}

			ErrorOr<OrderCheckModel> savedOrder =
				await _cartService.SaveOrderAsync(orderRequest.ToModel());

			return savedOrder.Match(
				order => Results.Ok(order),
				errors => Results.BadRequest(errors.First())
			);
		}
	}
}
