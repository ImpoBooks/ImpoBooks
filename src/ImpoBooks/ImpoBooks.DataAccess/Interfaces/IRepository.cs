using ImpoBooks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoBooks.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseModelExtended
	{
        Task CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task DeleteAsync(TEntity entity);

        Task DeleteByIdAsync(int id);

		Task UpdateAsync(TEntity entity);
    }
}
