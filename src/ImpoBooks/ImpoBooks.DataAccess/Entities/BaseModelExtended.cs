using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities
{
	public class BaseModelExtended : BaseModel
	{
		[PrimaryKey("id", true)] public int Id { get; set; }
	}
}
