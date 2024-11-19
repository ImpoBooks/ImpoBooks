using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Comments")]
	public class Comment : BaseModelExtended, IAutoInc<AutoIncComment>
	{
		[Column("user_id")] public int UserId { get; set; }
		[Column("content")] public string Content { get; set; }
		[Column("likes_number")] public int LikesNumber { get; set; }
		[Column("dislikes_number")] public int DislikesNumber { get; set; }
		[Reference(typeof(User))] public User User { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Comment comment &&
			Id == comment.Id &&
			UserId == comment.UserId &&
			Content == comment.Content &&
			LikesNumber == comment.LikesNumber &&
			DislikesNumber == comment.DislikesNumber &&
			User.Name == comment.User.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		AutoIncComment IAutoInc<AutoIncComment>.ToAutoInc()
		{
			return new AutoIncComment()
			{
				Id = Id,
				UserId = UserId,
				Content = Content,
				LikesNumber = LikesNumber,
				DislikesNumber = DislikesNumber,
				User = User
			};
		}
	}
}
