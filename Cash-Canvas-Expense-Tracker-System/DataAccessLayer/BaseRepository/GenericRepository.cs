using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CashCanvas.Data.BaseRepository
{
    /// <summary>
    /// Generic repository implementation for standard CRUD operations.
    /// 
    /// Usage:
    /// - Use AddAsync, Update, and Remove to track changes.
    /// - Changes are NOT saved to the database until UnitOfWork.CompleteAsync() is called.
    /// - Fetch methods support filtering, projection, and eager loading (includes).
    /// - Use asNoTracking=true for read-only queries to improve performance.
    /// </summary>
    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        // Fetching single/multiple entities
        public virtual async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        // Adding, updating, removing entities (tracked, not saved)
        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
        public virtual void Update(T entity) => _dbSet.Update(entity);
        public virtual void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

        // Advanced queries with projection and tracking options
        public virtual async Task<TResult?> GetFirstOrDefaultAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.Where(predicate).Select(selector).FirstOrDefaultAsync();
        }

        public virtual async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<List<TResult>> GetListAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            return await query.Where(predicate).Select(selector).ToListAsync();
        }

        public virtual async Task<List<T>> GetListAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            return await query.Where(predicate).ToListAsync();
        }
        // Utility methods
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate != null)
                return await _dbSet.CountAsync(predicate);
            return await _dbSet.CountAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);
    }

}
