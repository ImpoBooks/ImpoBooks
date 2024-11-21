using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.BusinessLogic.Services.Product;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Requests;
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
				product => Results.Ok(product),
				errors => Results.BadRequest(errors.First())
			);
		}

		[HttpPost("{id}/comment")]
		[ProducesResponseType<CommentModel>(StatusCodes.Status201Created)]
		[ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
		public async Task<IResult> CreateComment(int id, CommentCreateRequest commentRequest) 
		{
			ErrorOr<CommentModel> result =
				await _productService.AddCommentAsync(id, commentRequest.ToModel());

			return result.Match(
				comment => Results.Ok(comment),
				errors => Results.BadRequest(errors.First())
			);
		}
	}
}
