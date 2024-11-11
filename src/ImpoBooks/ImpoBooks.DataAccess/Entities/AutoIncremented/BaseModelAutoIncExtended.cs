using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Entities.AutoIncremented
{
	public class BaseModelAutoIncExtended : BaseModel
	{
		[PrimaryKey("id", false)] public int Id { get; set; }
	}
}
