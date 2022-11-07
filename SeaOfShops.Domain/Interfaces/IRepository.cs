using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaOfShops.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public DbSet<T> Entities { get; }
        public DbContext DbContext { get; }
        public Task<IList<T>> GetAllAsync();
        public T Find(params object[] keyValues);
        public Task<T> FindAsync(params object[] keyValues);
        public Task InsertAsync(T entity, bool saveChanges = true);
        public Task InsertRangeAsync(IEnumerable<T> entities, bool saveChanges = true);        
        public Task DeleteAsync(int id, bool saveChanges = true);        
        public Task DeleteAsync(T entity, bool saveChanges = true);
        public Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true);
    }
}
