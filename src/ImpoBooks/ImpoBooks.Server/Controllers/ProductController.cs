using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.BusinessLogic.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController(IProductService productService) : Controller
	{
		private readonly IProductService _productService = productService;

		[HttpGet("{id}")]
		[ProducesResponseType<ProductModel>(StatusCodes.Status200OK)]
		[ProducesResponseType<List<Error>>(StatusCodes.Status404NotFound)]
		public async Task<IResult> GetProduct(int id)
		{
			ErrorOr<ProductModel> result = await _productService.GetProductAsync(id);

			return result.Match(
				books => Results.Ok(books),
				errors => Results.BadRequest(errors.First())
			);
		}
	}
}
