namespace Eshop.Api.Repositories;

/// <summary>
/// Generic repository interface for common CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets all entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    Task DeleteAsync(T entity);
}
