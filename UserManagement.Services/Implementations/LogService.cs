using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public async Task<Log?> GetByIdAsync(long id) => await _dataAccess.FindAsync<Log>(id);
    public async Task<IEnumerable<Log>> GetAllAsync() => await _dataAccess.GetAllAsync<Log>();
    public async Task<IEnumerable<Log>> FilterByTypeAsync(LogType type) => (await _dataAccess.GetAllAsync<Log>()).Where(l => l.Type == type);
}
