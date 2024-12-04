using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.Tests.Integration.Seeds
{
	public static class CommentSeeder
	{
		public static IEnumerable<Comment> PreparedComments =>
			new Comment[]
			{
				new()
				{
					Id = 1,
					UserId = 1,
					ProductId = 1,
					Content = "Cool book",
					LikesNumber = 7,
					DislikesNumber = 1,
					Rating = 4.5M
				},
				new() {
					Id = 2,
					UserId = 2,
					ProductId = 2,
					Content = "I love this book",
					LikesNumber = 9,
					DislikesNumber = 0,
					Rating = 4.7M
				},
				new()
				{
					Id = 3,
					UserId = 2,
					ProductId = 3,
					Content = "All recommend this one",
					LikesNumber = 8,
					DislikesNumber = 0,
					Rating = 4.3M
				},
				new()
				{
					Id = 4,
					UserId = 3,
					ProductId = 4,
					Content = "When will there be a sequel?",
					LikesNumber = 11,
					DislikesNumber = 2,
					Rating = 4.5M
				},
				new()
				{
					Id = 5,
					UserId = 4,
					ProductId = 2,
					Content = "I don`t like it",
					LikesNumber = 1,
					DislikesNumber = 5,
					Rating = 3.3M
				},

			};

		public static string Seed =
			"INSERT INTO \"Comments\" (id, user_id, content, likes_number, dislikes_number, rating, product_id) VALUES" +
			string.Join(", ", PreparedComments.Select(c => $"(" +
			$"{c.Id}," +
			$"{c.UserId}," +
			$"'{c.Content}'," +
			$"{c.LikesNumber}," +
			$"{c.DislikesNumber}," +
			$"{c.Rating.ToString().Replace(",", ".")}," +
			$"{c.ProductId})")) +
			";";
	}
}
