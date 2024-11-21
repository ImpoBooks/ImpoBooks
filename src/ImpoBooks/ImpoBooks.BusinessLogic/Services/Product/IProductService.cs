using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Product
{
	public interface IProductService
	{
		Task<ErrorOr<ProductModel>> GetProductAsync(int productId);
		Task<ErrorOr<CommentModel>> AddCommentAsync(int productId, ProductCommentModel comment);
		Task<ErrorOr<CommentModel>> IncrementLikeNumberAsync(int commentId);
		Task<ErrorOr<CommentModel>> IncrementDislikeNumberAsync(int commentId);
	}
}
