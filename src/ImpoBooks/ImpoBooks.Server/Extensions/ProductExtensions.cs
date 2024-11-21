using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.Server.Requests;

namespace ImpoBooks.Server.Extensions
{
	public static class ProductExtensions
	{
		public static ProductCommentModel ToModel(this CommentCreateRequest source)
		{
			return new ProductCommentModel()
			{
				UserId = source.UserId,
				Content = source.Content,
				Rating = source.Rating
			};
		}
	}
}
