using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Mapping;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Product
{
	public class ProductService(IProductRepository productRepository) : IProductService
	{
		private readonly IProductRepository _productRepository = productRepository;

		public async Task<ErrorOr<ProductModel>> GetProductAsync(int productId)
		{
			if (productId is 0)
				return ProductErrors.ProductIdIsZero;

			DataAccess.Entities.Product product = await _productRepository.GetByIdAsync(productId);
			if (product is null)
				return ProductErrors.ProductNotFound;

			ProductModel result = product.ToProductModel();

			return result.ToErrorOr();
		}
	}
}
