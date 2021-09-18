using Dyo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dyo.Core.DataAccess
{
    public interface IEntityRepository<T> where T: class, IEntity, new()
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(Expression<Func<T, bool>> filter, T entity, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(Expression<Func<T, bool>> filter);


    }
}
