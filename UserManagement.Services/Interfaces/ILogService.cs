using System.Collections.Generic;
using UserManagement.Data.Enums;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;
public interface ILogService
{
    /// <summary>
    /// Get log by unique identifier
    /// </summary>
    /// <param name="id">Unique identifier of log</param>
    /// <returns>Log if found, otherwise null</returns>
    Log? GetById(long id);

    /// <summary>
    /// Get all logs
    /// </summary>
    /// <returns>All logs</returns>
    IEnumerable<Log> GetAll();

    /// <summary>
    /// Get logs filtered by type
    /// </summary>
    /// <param name="type">Enum corresponding to user action type</param>
    /// <returns>Users matching type</returns>
    IEnumerable<Log> FilterByType(LogType type);
}
