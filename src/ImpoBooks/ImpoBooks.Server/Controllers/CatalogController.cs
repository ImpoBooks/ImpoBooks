using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Catalog;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Requests;
using ImpoBooks.Server.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CatalogController(ICatalogService catalogService) : Controller
	{
		private readonly ICatalogService _catalogService = catalogService;

		[HttpGet]
		[ProducesResponseType<IEnumerable<CatalogBookModel>>(StatusCodes.Status200OK)]
		[ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
		public async Task<IResult> GetBooks(CatalogRequest catalogRequest)
		{
			ErrorOr<IEnumerable<CatalogBookModel>> result =
				await _catalogService.GetBooksAsync(catalogRequest.ToModel());

			return result.Match(
				books => Results.Ok(books),
				errors => Results.BadRequest(errors.First())
			);
		}

		[HttpPost("admin/books/create")]
		[ProducesResponseType<CatalogBookModel>(StatusCodes.Status201Created)]
		[ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
		public async Task<IResult> CreateBook(CatalogCreateRequest catalogRequest)
		{
			ErrorOr<CatalogBookModel> result =
				await _catalogService.CreateBookAsync(catalogRequest.ToModel());

			return result.Match(
				books => Results.Ok(books),
				errors => Results.BadRequest(errors.First())
			);
		}
	}
}
