using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public User? GetById(long id) => _dataAccess.Find<User>(id);
    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public IEnumerable<User> FilterByActive(bool isActive) => _dataAccess.GetAll<User>().Where(x => x.IsActive == isActive);
    public void Update(User user) => _dataAccess.Update(user);
    public void Delete(User user) => _dataAccess.Delete(user);
}
