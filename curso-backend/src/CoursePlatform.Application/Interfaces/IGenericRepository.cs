using System.Linq.Expressions;

namespace CoursePlatform.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetByIdAsync(string id); // For Identity User key
    Task<IEnumerable<T>> GetAllAsync();
    
    // Advanced search
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "");

    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        string includeProperties = "");

    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task DeleteAsync(Guid id);
}
