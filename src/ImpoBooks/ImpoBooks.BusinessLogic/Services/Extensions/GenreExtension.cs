using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Extensions
{
	public static class GenreExtension
	{
		public async static Task<Genre> CreateIfNotExistAsync(this Genre source, IGenreRepository genreRepository, string name)
		{
			if (source is null)
			{
				await genreRepository.CreateAsync(new Genre()
				{
					Name = name
				});

				return await genreRepository.GetByNameAsync(name);
			}

			return source;
		}
	}
}
