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

		public static Error CommentIdIsZero =>
			Error.Custom(ErrorTypes.WrongInfo, "Comment.Id.WrongInfo", "Comment id was equal to zero.");

		public static Error CommentContentWrongInfo =>
			Error.Custom(ErrorTypes.WrongInfo, "Comment.Content.WrongInfo", "Comment content has null or empty value.");

		public static Error ProductNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Product.NotFound", "Failed to get product.");

		public static Error CommentNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Comment.NotFound", "Failed to get comment.");

		public static Error UserNotFound =>
			Error.Custom(ErrorTypes.NotFound, "User.NotFound", "User with this id does not exist.");

		public static Error NewCommentNotFound =>
			Error.Custom(ErrorTypes.NotFound, "Comment.NotFound", "Attempting to create a new comment is unsuccessful.");

		public static Error CommentIsNull =>
			Error.Custom(ErrorTypes.IsNull, "Comment.IsNull", "Provided comment has null value.");

		public static Error LikeNotAdded =>
			Error.Custom(ErrorTypes.WrongInfo, "Like.NotAdded", "Attempting to increase like number is unsuccessful.");

		public static Error DislikeNotAdded =>
			Error.Custom(ErrorTypes.WrongInfo, "Dislike.NotAdded", "Attempting to increase dislike number is unsuccessful.");
	}
}
