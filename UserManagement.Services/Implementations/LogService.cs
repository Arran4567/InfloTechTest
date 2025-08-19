using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Implementations;
public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public Log? GetById(long id) => _dataAccess.Find<Log>(id);
    public IEnumerable<Log> GetAll() => _dataAccess.GetAll<Log>();
    public IEnumerable<Log> FilterByType(LogType type) => _dataAccess.GetAll<Log>().Where(l => l.Type == type);
}
