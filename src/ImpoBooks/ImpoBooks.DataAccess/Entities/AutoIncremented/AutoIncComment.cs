using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	[Table("Comments")]
	public class AutoIncComment : BaseModelAutoIncExtended
	{
		[Column("user_id")] public int UserId { get; set; }
		[Column("content")] public string Content { get; set; }
		[Column("likes_number")] public int LikesNumber { get; set; }
		[Column("dislikes_number")] public int DislikesNumber { get; set; }
		[Reference(typeof(User))] public User User { get; set; }
	}
}
