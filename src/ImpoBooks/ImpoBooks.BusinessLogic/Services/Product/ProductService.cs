using ErrorOr;
using ImpoBooks.BusinessLogic.Services.Mapping;
using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using ImpoBooks.Infrastructure.Errors.Product;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Product
{
	public class ProductService(IProductRepository productRepository,
		ICommentRepository commentRepository,
		IUsersRepository userRepository) : IProductService
	{
		private readonly IProductRepository _productRepository = productRepository;
		private readonly ICommentRepository _commentRepository = commentRepository;
		private readonly IUsersRepository _userRepository = userRepository;


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

		public async Task<ErrorOr<CommentModel>> AddCommentAsync(int productId, ProductCommentModel comment)
		{
			if (productId is 0)
				return ProductErrors.ProductIdIsZero;

			DataAccess.Entities.Product product = await _productRepository.GetByIdAsync(productId);
			if (product is null)
				return ProductErrors.ProductNotFound;

			if(comment is null)
				return ProductErrors.CommentIsNull;

			if (comment.Content.IsNullOrEmpty())
				return ProductErrors.CommentContentWrongInfo;

			User user = await _userRepository.GetByIdAsync(comment.UserId);
			if (user is null)
				return ProductErrors.UserNotFound;

			IEnumerable<Comment>? userComments = await _commentRepository.GetByUserName(user.Name);

			Comment newComment = new Comment()
			{
				UserId = comment.UserId,
				ProductId = productId,
				Content = comment.Content,
				LikesNumber = 0,
				DislikesNumber = 0,
				Rating = comment.Rating,
			};

			await _commentRepository.CreateAsync(newComment);

			IEnumerable<Comment>? updatedUserComments = await _commentRepository.GetByUserName(user.Name);
			if(userComments.Count() + 1 != updatedUserComments.Count())
				return ProductErrors.NewCommentNotFound;

			CommentModel result = updatedUserComments.FirstOrDefault(c => c.Content == newComment.Content)!.ToCommentModel();

			return result.ToErrorOr();
		}
	}
}
