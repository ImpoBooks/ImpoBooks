using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class CommentModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Content { get; set; }
		public int LikesNumber { get; set; }
		public int DislikesNumber { get; set; }
		public decimal Rating { get; set; }

		public override bool Equals(object obj)
		{
			return obj is CommentModel comment &&
			Id == comment.Id &&
			UserName == comment.UserName &&
			Content == comment.Content &&
			LikesNumber == comment.LikesNumber &&
			DislikesNumber == comment.DislikesNumber &&
			Rating == comment.Rating;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}
