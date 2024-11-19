using ImpoBooks.DataAccess.Entities.AutoIncremented;
using Microsoft.IdentityModel.Tokens;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using System.Xml.Linq;
using ImpoBooks.DataAccess.Interfaces;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Products")]
	public class Product : BaseModelExtended, IAutoInc<AutoIncProduct>
	{
		[Column("book_id")] public int BookId { get; set; }
		[Reference(typeof(Book))] public Book Book { get; set; }
		[Column("comments")] public ICollection<Comment> Comments { get; set; }
		public override bool Equals(object obj)
		{
			return obj is Product product &&
			Id == product.Id &&
			BookId == product.BookId &&
			Book.Equals(product.Book);
			//Comments?.Count == product.Comments?.Count;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		public AutoIncProduct ToAutoInc()
		{
			return new AutoIncProduct()
			{
				Id = Id,
				BookId = BookId,
				Book = Book,
				Comments = Comments
			};
		}
	}
}
