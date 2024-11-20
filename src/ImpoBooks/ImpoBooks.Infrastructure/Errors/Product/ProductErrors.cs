using ErrorOr;
using ImpoBooks.BusinessLogic.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Infrastructure.Errors.Product
{
	public static class ProductErrors
	{
		public static Error ProductIdIsZero =>
			Error.Custom(ErrorTypes.WrongInfo, "Product.Id.WrongInfo", "Product id was equal to zero.");

		public static Error ProductNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Product.NotFound", "Failed to get product.");
	}
}
