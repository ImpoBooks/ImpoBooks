using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.DataAccess.Repositories;

namespace ImpoBooks.Server.Extensions
{
	public static class AppServicesExtensions
	{
		public static IServiceCollection AddRepository(this IServiceCollection services)
		{
			services.AddSingleton<IUsersRepository, UsersRepository>();
			services.AddSingleton<IAuthorRepository, AuthorRepository>();
			services.AddSingleton<IBookRepository, BookRepository>();
			services.AddSingleton<IBookGenreRepository, BookGenreRepository>();
			services.AddSingleton<IGenreRepository, GenreRepository>();
			services.AddSingleton<IPersonRepository, PersonRepository>();
			services.AddSingleton<IPublisherRepository, PublisherRepository>();

			return services;
		}
	}
}
