using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Data.Enums;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Get user by unique identifier
    /// </summary>
    /// <param name="id">Unique identifier of user</param>
    /// <returns>User if found, otherwise null</returns>
    User? GetById(string id);

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>All users</returns>
    IEnumerable<User> GetAll();

    /// <summary>
    /// Get users filtered by active status
    /// </summary>
    /// <param name="isActive">True to get active users, false for inactive</param>
    /// <returns>Users matching active state</returns>
    IEnumerable<User> FilterByActive(bool isActive);

    /// <summary>
    /// Creates new user
    /// </summary>
    /// <param name="user">User to create</param>
    void Create(User user);

    /// <summary>
    /// Updates existing user
    /// </summary>
    /// <param name="user">User to update</param>
    void Update(User user);

    /// <summary>
    /// Removes user
    /// </summary>
    /// <param name="user">User to delete</param>
    void Delete(User user);

    /// <summary>
    /// Adds user log
    /// </summary>
    /// <param name="user">User to add log to</param>
    /// <param name="type">Action performed on user</param>
    void AddLog(string id, LogType type);

    /// <summary>
    /// Get user by unique identifier
    /// </summary>
    /// <param name="id">Unique identifier of user</param>
    /// <returns>User if found, otherwise null</returns>
    Task<User?> GetByIdAsync(string id);

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>All users</returns>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>
    /// Get users filtered by active status
    /// </summary>
    /// <param name="isActive">True to get active users, false for inactive</param>
    /// <returns>Users matching active state</returns>
    Task<IEnumerable<User>> FilterByActiveAsync(bool isActive);

    /// <summary>
    /// Creates new user
    /// </summary>
    /// <param name="user">User to create</param>
    Task CreateAsync(User user);

    /// <summary>
    /// Updates existing user
    /// </summary>
    /// <param name="user">User to update</param>
    Task UpdateAsync(User user);

    /// <summary>
    /// Removes user
    /// </summary>
    /// <param name="user">User to delete</param>
    Task DeleteAsync(User user);

    /// <summary>
    /// Adds user log
    /// </summary>
    /// <param name="user">User to add log to</param>
    /// <param name="type">Action performed on user</param>
    Task AddLogAsync(User user, LogType type);
}
