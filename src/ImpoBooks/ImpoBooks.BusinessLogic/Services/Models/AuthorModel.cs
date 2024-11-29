using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class AuthorModel
	{
		public string Name { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is AuthorModel author &&
			Name == author.Name;
		}
	}
}
