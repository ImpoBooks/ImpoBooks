using ImpoBooks.BusinessLogic.Services.Models;
using ImpoBooks.DataAccess.Entities;
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
				Genres = string.Join(", ", genres),
				ReleaseDate = source.ReleaseDate,
				Rating = source.Rating,
				Price = source.Price,
			};
		}
	}
}
