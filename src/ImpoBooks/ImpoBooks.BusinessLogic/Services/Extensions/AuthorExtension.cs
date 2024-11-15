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
	public static class AuthorExtension
	{
		public async static Task<Author> CreateIfNotExistAsync(this Author source, IAuthorRepository authorRepository, IPersonRepository personRepository, string name, string surname)
		{
			if (source is null)
			{
				await personRepository.CreateAsync(new Person()
				{
					Name = name,
					Surname = surname
				});

				IEnumerable<Person> person = await personRepository.GetByFullNameAsync(name, surname);
				await authorRepository.CreateAsync(new Author()
				{
					PersonId = person.First().Id,
				});

				return await authorRepository.GetByFullNameAsync(name, surname);
			}

			return source;
		}
	}
}
