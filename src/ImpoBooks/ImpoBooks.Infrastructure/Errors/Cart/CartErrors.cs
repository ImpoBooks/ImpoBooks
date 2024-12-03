using ErrorOr;
using ImpoBooks.BusinessLogic.Errors;

namespace ImpoBooks.Infrastructure.Errors.Cart
{
	public static class CartErrors
	{
		public static Error OrderProductsIsNull =>
			Error.Custom(ErrorTypes.IsNull, "OrderProducts.IsNull", "Order product models was null.");
		public static Error RequisitesIsNull =>
			Error.Custom(ErrorTypes.IsNull, "Requisites.IsNull", "Customer requisites model was null.");
		public static Error OrderModelIsNull =>
			Error.Custom(ErrorTypes.IsNull, "OrderModel.IsNull", "Order model object was null.");
		public static Error ProductNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Product.NotFound", "Failed to find order product in DB.");
		public static Error OrderNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Order.NotFound", "Failed to get order.");
		public static Error OrderCodeAlreadyExists =>
			Error.Custom(ErrorTypes.AlreadyExists, "OrderCode.AlreadyExists", "Order with such code already exists.");
	}
}
