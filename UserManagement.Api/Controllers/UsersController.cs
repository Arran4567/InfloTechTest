using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Models.Logs;
using UserManagement.Api.Models.Users;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using Hangfire;

namespace UserManagement.Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    #region Public Methods

    #region HttpGet

    [HttpGet("")]
    [HttpGet("list")]
    public IActionResult List(bool? filter = null)
    {
        var users = filter.HasValue ? _userService.FilterByActive(filter.Value) : _userService.GetAll();

        return Ok(UserListToViewModel(users));
    }

    [HttpGet("detail/{id}")]
    public IActionResult Detail(string id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return Ok("User not found.");
        }

        BackgroundJob.Enqueue(() => _userService.AddLog(user.Id, LogType.View));
        return Ok(UserToViewModel(user));
    }

    #endregion

    #region HttpPost

    [HttpPost("Create")]
    public IActionResult Create(UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth,
            IsActive = model.IsActive,
        };

        _userService.Create(user);
        BackgroundJob.Enqueue(() => _userService.AddLog(user.Id, LogType.Create));
        return Ok(UserToViewModel(user));
    }

    [HttpPut("edit")]
    public IActionResult Edit(UserListItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if(model.Id == null)
        {
            return BadRequest("Invalid User.");
        }

        var user = _userService.GetById(model.Id);
        user!.Forename = model.Forename;
        user!.Surname = model.Surname;
        user!.Email = model.Email;
        user!.DateOfBirth = model.DateOfBirth;
        user!.IsActive = model.IsActive;

        _userService.Update(user);
        BackgroundJob.Enqueue(() => _userService.AddLog(user.Id, LogType.Update));
        return Ok(UserToViewModel(user));
    }
    #endregion

    #region HttpDelete

    [HttpDelete("delete/{id}")]
    public IActionResult Delete(string id)
    {
        var entityToDelete = _userService.GetById(id);

        if (entityToDelete == null)
        {
            return BadRequest("User not found.");
        }

        BackgroundJob.Enqueue(() => _userService.AddLog(entityToDelete.Id, LogType.Delete));
        _userService.Delete(entityToDelete);
        return Ok();
    }

    #endregion

    #region Private Methods

    private UserListViewModel UserListToViewModel(IEnumerable<User> users)
    {
        var items = users.Select(u => UserToViewModel(u)).ToList();
        return new UserListViewModel { Items = items };
    }

    private UserListItemViewModel UserToViewModel(User user)
    {
        return new UserListItemViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            Logs = MapLogs(user.Logs)
        };
    }

    private LogListViewModel MapLogs(IEnumerable<Log> logs) =>
    new LogListViewModel
    {
        Items = logs.Select(l => new LogListItemViewModel
        {
            Id = l.Id,
            Type = l.Type,
            Description = l.Description,
            DateTime = l.DateTime
        }).ToList()
    };

    #endregion

    #endregion
}
