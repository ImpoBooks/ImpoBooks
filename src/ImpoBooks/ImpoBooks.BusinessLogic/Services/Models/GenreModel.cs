using ImpoBooks.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.BusinessLogic.Services.Models
{
	public class GenreModel
	{
		public string Name { get; set; }
		public override bool Equals(object obj)
		{
			return obj is GenreModel book &&
			Name == book.Name;

		}
	}
}
