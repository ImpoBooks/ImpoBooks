using Microsoft.IdentityModel.Tokens;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Authors")]
	public class Author : BaseModelExtended
	{
		[Column("person_id")] public int PersonId { get; set; }
		[Reference(typeof(Person))] public Person Person { get; set; }
		[Column("books")] public ICollection<Book>? Books { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Author author &&
			Id == author.Id &&
			PersonId == author.PersonId &&
			Person.Id == author.Person.Id &&
			Person.Name == author.Person.Name &&
			Person.Surname == author.Person.Surname &&
			(Books?.Count == author.Books?.Count ||
			Books.IsNullOrEmpty() == author.Books?.IsNullOrEmpty());
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}
