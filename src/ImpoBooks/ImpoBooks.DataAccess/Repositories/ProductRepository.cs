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
	public class ProductRepository(Client client) : Repository<Product, AutoIncProduct>(client), IProductRepository
	{
		private readonly Client _client = client;
		public override async Task<Product> GetByIdAsync(int id)
		{
			ModeledResponse<Product> responseP = await _client.From<Product>().Where(x => x.Id == id).Get();
			Product product = responseP.Model;
			if (product is null) return product;

			ModeledResponse<Comment> responseC = await _client.From<Comment>().Where(c => c.ProductId == product.Id).Get();
			product.Comments = responseC.Models;

			await SetGenres(product.Book);

			return product;
		}

		public override async Task<IEnumerable<Product>> GetAllAsync()
		{
			ModeledResponse<Product> responseP = await _client.From<Product>().Get();
			IEnumerable<Product> products = responseP.Models;

			foreach (Product product in products)
			{
				ModeledResponse<Comment> responseC = await _client.From<Comment>().Where(c => c.ProductId == product.Id).Get();
				product.Comments = responseC.Models;

				await SetGenres(product.Book);
			}

			return products;
		}

		private async Task SetGenres(Book book)
		{
			ModeledResponse<BookGenre> responseBG = await _client.From<BookGenre>()
				.Select("*, Genres(*))")
				.Filter("book_id", Operator.Equals, book.Id)
				.Get();

			IEnumerable<BookGenre> relation = responseBG.Models;
			ICollection<Genre> genres = relation.Select(x => x.Genre).ToList();
			book.Genres = genres;
		}
	}
}
