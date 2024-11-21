using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;
using Supabase.Postgrest.Responses;
using static Supabase.Postgrest.Constants;

namespace ImpoBooks.DataAccess.Repositories
{
	public class CommentRepository(Client client) : Repository<Comment, AutoIncComment>(client), ICommentRepository
	{
		private readonly Client _client = client;

		public async Task<IEnumerable<Comment>> GetByUserName(string name)
		{
			ModeledResponse<Comment> response = await _client.From<Comment>()
				.Select("*, Users!inner(*))")
				.Filter("Users.name", Operator.Equals, name)
				.Get();

			IEnumerable<Comment> comments = response.Models;
			return comments;
		}
	}
}
