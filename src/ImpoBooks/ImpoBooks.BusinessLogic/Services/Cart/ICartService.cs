using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Cart
{
	public interface ICartService
	{
		Task<ErrorOr<Success>> CheckAvailabilityAsync(IEnumerable<OrderProductModel> products);
		Task<ErrorOr<Success>> SendToPaymentServiceAsync(CustomerRequisitesModel requisites);
		Task<ErrorOr<OrderCheckModel>> SaveOrderAsync(OrderModel orderModel);
	}
}
