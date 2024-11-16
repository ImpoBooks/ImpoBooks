using ImpoBooks.DataAccess.Entities.AutoIncremented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Interfaces
{
	public interface IAutoInc<T>
	{
		public T ToAutoInc();
	}
}
