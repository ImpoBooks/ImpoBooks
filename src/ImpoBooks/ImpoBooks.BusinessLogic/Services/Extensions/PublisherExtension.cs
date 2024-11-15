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
	public static class PublisherExtension
	{
		public async static Task<Publisher> CreateIfNotExistAsync(this Publisher source, IPublisherRepository publisherRepository, string name)
		{
			if (source is null)
			{
				await publisherRepository.CreateAsync(new Publisher()
				{
					Name = name
				});

				return await publisherRepository.GetByNameAsync(name);
			}
			
			return source;
		}
	}
}
