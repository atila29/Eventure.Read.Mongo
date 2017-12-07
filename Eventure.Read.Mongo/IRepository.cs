using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eventure.ReadModel;
using MongoDB.Driver;

namespace Eventure.Read.Mongo
{
    public interface IRepository<T, TId> : IQueryable<T> where T : IReadModel<TId>
    {
        IMongoCollection<T> Collection { get; }

        Task<T> GetByIdAsync(TId id);

        Task<T> AddAsync(T entity);

        Task AddAsync(IEnumerable<T> entities);

        Task<T> UpdateAsync(T entity);

        Task UpdateAsync(IEnumerable<T> entities);

        Task DeleteAsync(TId id);

        Task DeleteAsync(T entity);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        Task DeleteAllAsync();

        Task<long> CountAsync();

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}