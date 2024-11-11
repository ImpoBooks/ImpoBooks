using ImpoBooks.DataAccess.Entities.AutoIncremented;
using ImpoBooks.DataAccess.Interfaces;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace ImpoBooks.DataAccess.Entities
{
	[Table("Persons")]
	public class Person : BaseModelExtended, IAutoInc<AutoIncPerson>
	{
		[Column("name")] public string Name { get; set; }
		[Column("surname")] public string Surname { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Person person &&
					Id == person.Id &&
					Name == person.Name &&
					Surname == person.Surname;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		public AutoIncPerson ToAutoInc()
		{
			return new AutoIncPerson()
			{
				Id = Id,
				Name = Name,
				Surname = Surname
			};
		}
	}
}
