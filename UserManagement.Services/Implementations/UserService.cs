using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public User? GetById(long id)
    {
        var user = _dataAccess.GetAll<User>()
            .Include(u => u.Logs)
            .FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return null;
        }

        return user;
    }
    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public IEnumerable<User> FilterByActive(bool isActive) => _dataAccess.GetAll<User>().Where(x => x.IsActive == isActive);

    public void Create(User user)
    {
        _dataAccess.Create(user);
    }
    public void Update(User user)
    {
        _dataAccess.Update(user);
    }
    public void Delete(User user)
    {
        _dataAccess.Delete(user);
    }

    public void AddLog(ref User user, LogType type)
    {
        var log = new Log
        {
            Type = type,
            Description = type.ToDescription($"{user.Forename} {user.Surname}"),
            User = user,
        };

        user.Logs.Add(log);
        _dataAccess.Update(user);
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        var user = (await _dataAccess.GetAllAsync<User>())
            .Include(u => u.Logs)
            .FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return null;
        }

        return user;
    }
    public async Task<IEnumerable<User>> GetAllAsync() => await _dataAccess.GetAllAsync<User>();
    public async Task<IEnumerable<User>> FilterByActiveAsync(bool isActive) => (await _dataAccess.GetAllAsync<User>()).Where(x => x.IsActive == isActive);

    public async Task CreateAsync(User user)
    {
        await _dataAccess.CreateAsync(user);
    }
    public async Task UpdateAsync(User user)
    {
        await _dataAccess.UpdateAsync(user);
    }
    public async Task DeleteAsync(User user)
    {
        await _dataAccess.DeleteAsync(user);
    }

    public async Task AddLogAsync(User user, LogType type)
    {
        var log = new Log
        {
            Type = type,
            Description = type.ToDescription($"{user.Forename} {user.Surname}"),
            User = user,
        };

        user.Logs.Add(log);
        await _dataAccess.UpdateAsync(user);
    }
}
