using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Data;
public interface IDataContext
{
    /// <summary>
    /// Get specific items
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="id">Unique identifier of entity</param>
    /// <returns>Entity if found, otherwise null</returns>
    TEntity? Find<TEntity>(object id) where TEntity : class;

    /// <summary>
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity">Type of entities</typeparam>
    /// <returns>An <see cref="IQueryable{TEntity}"/>Queries all entities</returns>
    IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

    /// <summary>
    /// Adds new entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity to create</param>
    void Create<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Updates entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity to update</param>
    void Update<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Removes entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity to delete</param>
    void Delete<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Get specific items
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="id">Unique identifier of entity</param>
    /// <returns>Entity if found, otherwise null</returns>
    Task<TEntity?> FindAsync<TEntity>(object id) where TEntity : class;

    /// <summary>
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity">Type of entities</typeparam>
    /// <returns>An <see cref="IQueryable{TEntity}"/>Queries all entities</returns>
    Task<IQueryable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

    /// <summary>
    /// Adds new entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity to create</param>
    Task CreateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Updates entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity to update</param>
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Removes entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <param name="entity">Entity to delete</param>
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
}
