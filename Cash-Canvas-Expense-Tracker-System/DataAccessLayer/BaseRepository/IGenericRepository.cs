using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace CashCanvas.Data.BaseRepository;
/// <summary>
/// Generic repository interface for standard data operations.
/// Use Add/Update/Remove for tracking, then call UnitOfWork.CompleteAsync() to save.
/// </summary>
public interface IGenericRepository<T> where T : class
{
    // --- Fetching single/multiple entities ---
    Task<T?> GetByIdAsync(object id);                      // Get by primary key
    Task<IEnumerable<T>> GetAllAsync();                    // Get all records
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate); // Filtered list

    // --- Adding, updating, removing entities (tracked, not saved) ---
    Task AddAsync(T entity);                               // Add new entity
    Task AddRangeAsync(IEnumerable<T> entities);           // Add multiple new entities
    void Update(T entity);                                 // Update one
    void UpdateRange(IEnumerable<T> entities);             // Update many
    
    // void Remove(T entity);                                 // Remove one
    // void RemoveRange(IEnumerable<T> entities);             // Remove many

    // --- Advanced queries with projection, includes, and tracking options ---
    Task<TResult?> GetFirstOrDefaultAsync<TResult>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TResult>> selector,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool asNoTracking = true);
    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool asNoTracking = true);

    Task<List<TResult>> GetListAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = true);

    Task<List<T>> GetListAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = true);       

    // --- Utility methods ---
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null); // Count results
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);       // Check existence
}