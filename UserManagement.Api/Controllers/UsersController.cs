using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Models.Users;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Api.Controllers;

[Route("api/users")]
[ApiController]
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

        return Ok(users);
    }

    [HttpGet("detail/{id}")]
    public IActionResult Detail(string id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return Ok("User not found.");
        }

        _userService.AddLog(ref user, LogType.View);
        return Ok(user);
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
        _userService.AddLog(ref user, LogType.Create);
        return Ok(user);
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
        _userService.AddLog(ref user, LogType.Update);
        return Ok(user);
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

        _userService.AddLog(ref entityToDelete, LogType.Delete);
        _userService.Delete(entityToDelete);
        return Ok();
    }

    #endregion

    #endregion
}
