using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Catalog;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Request1s;
using ImpoBooks.Server.Requests;
using Microsoft.AspNetCore.Authorization;
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
		[ProducesResponseType<List<Error>>(StatusCodes.Status404NotFound)]
		public async Task<IResult> GetBooks([FromQuery] CatalogRequest catalogRequest)
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
				book => Results.Ok(book),
				errors => Results.BadRequest(errors.First())
			);
		}

		[HttpPut("admin/books/edit")]
		[ProducesResponseType<CatalogBookModel>(StatusCodes.Status200OK)]
		[ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
		public async Task<IResult> UpdateBook(CatalogUpdateRequest catalogRequest)
		{
			ErrorOr<CatalogBookModel> result =
				await _catalogService.UpdateBookAsync(catalogRequest.Id, catalogRequest.ToModel());

			return result.Match(
				book => Results.Ok(book),
				errors => Results.BadRequest(errors.First())
			);
		}

		[HttpDelete("admin/books/delete")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType<List<Error>>(StatusCodes.Status404NotFound)]
		public async Task<IResult> DeleteBook([FromBody] int id)
		{
			ErrorOr<Success> result =
				await _catalogService.DeleteBookAsync(id);

			return result.Match(
				_ => Results.Ok(),
				errors => Results.BadRequest(errors.First())
			);
		}
	}
}
