using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaOfShops.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();

    }
}
