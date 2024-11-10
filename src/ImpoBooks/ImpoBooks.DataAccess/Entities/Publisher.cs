using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Publishers")]
	public class Publisher : BaseModelExtended
	{
		[Column("name")] public string Name { get; set; }
		//[Reference(typeof(Book))] public ICollection<Book> Books { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Publisher publisher &&
			Id == publisher.Id &&
			Name == publisher.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}
