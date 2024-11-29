using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Entities.AutoIncremented;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Mapping
{
	public static class EntityToModel
	{
		public static CatalogBookModel ToCatalogBookModel(this Book source)
		{
			StringBuilder sb = new StringBuilder();
			string authorFullName = sb.Append(source.Author.Person.Name)
				.Append(" ")
				.Append(source.Author.Person.Surname)
				.ToString();

			IEnumerable<string> genres = source.Genres!.Select(g => g.Name);

			return new CatalogBookModel() 
			{
				Id = source.Id,
				Name = source.Name,
				Author = authorFullName,
				Genres = string.Join(" ", genres),
				ReleaseDate = source.ReleaseDate,
				Rating = source.Rating,
				Price = source.Price,
			};
		}

		public static CommentModel ToCommentModel(this Comment source)
		{
			return new CommentModel()
			{
				Id = source.Id,
				UserName = source.User.Name,
				Content = source.Content,
				LikesNumber = source.LikesNumber,
				DislikesNumber = source.DislikesNumber,
				Rating = source.Rating
			};
		}

		public static ProductModel ToProductModel(this DataAccess.Entities.Product source)
		{
			StringBuilder sb = new StringBuilder();
			string authorFullName = sb.Append(source.Book.Author.Person.Name)
				.Append(" ")
				.Append(source.Book.Author.Person.Surname)
				.ToString();

			IEnumerable<string> genres = source.Book.Genres!.Select(g => g.Name);

			return new ProductModel()
			{
				Id = source.Id,
				Name = source.Book.Name,
				Description = source.Book.Description,
				Format = source.Book.Format,
				Author = authorFullName,
				Publisher = source.Book.Publisher.Name,
				Genres = string.Join(" ", genres),
				ReleaseDate = source.Book.ReleaseDate,
				Rating = source.Book.Rating,
				Price = source.Book.Price,
				Comments = source.Comments.Select(c => c.ToCommentModel())
			};
		}

		public static OrderCheckModel ToOrderCheckModel(this Order source)
		{
			return new OrderCheckModel()
			{
				OrderCode = source.OrderCode,
				CreatedAt =	source.CreatedAt,
				FirstName = source.FirstName,
				LastName = source.LastName,
				Email = source.Email,
				Address = source.Address,
				City = source.City,
				ZipCode = source.ZipCode,
				Country = source.Country,
				TotalSum = source.TotalSum,
				ExpensesList = source.ExpensesList
			};
		}

		public static AuthorModel ToAuthorModel(this Author source)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(source.Person.Name)
				.Append(" ")
				.Append(source.Person.Surname);

			return new AuthorModel()
			{
				Name = sb.ToString()
			};
		}
	}
}
