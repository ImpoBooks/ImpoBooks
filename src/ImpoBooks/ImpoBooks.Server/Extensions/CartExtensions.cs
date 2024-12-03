using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.Server.Requests;

namespace ImpoBooks.Server.Extensions
{
	public static class CartExtensions
	{
		public static IEnumerable<OrderProductModel> ToOrderProductModelCollection(this CartOrderRequest source)
		{
			return source.Products.Select(p => new OrderProductModel() 
			{ 
				Id = p.Id,
				Count = p.Count 
			});
		}

		public static CustomerRequisitesModel ToRequisitesModel(this CartOrderRequest source)
		{
			return new CustomerRequisitesModel()
			{
				FirstName = source.FirstName,
				LastName = source.LastName,
				Email = source.Email,
				CardNumber = source.CardNumber,
				ExpirationDate = source.ExpirationDate,
				CVV = source.CVV,
				TotalSum = source.TotalSum
			};
		}

		public static OrderModel ToModel(this CartOrderRequest source)
		{
			return new OrderModel()
			{
				FirstName = source.FirstName,
				LastName = source.LastName,
				Email = source.Email,
				Address = source.Address,
				City = source.City,
				ZipCode = source.ZipCode,
				Country = source.Country,
				TotalSum = source.TotalSum,
				Products = source.ToOrderProductModelCollection()
			};
		}
	}
}
