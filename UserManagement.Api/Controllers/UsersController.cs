using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.WebMS.Controllers;

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

    [HttpGet("detail/{id:long}")]
    public IActionResult Detail(long id)
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
    public IActionResult Create(User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _userService.Create(model);
        _userService.AddLog(ref model, LogType.Create);
        return Ok(model);
    }

    [HttpPut("edit")]
    public IActionResult Edit(User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _userService.Update(model);
        _userService.AddLog(ref model, LogType.Update);
        return Ok(model);
    }
    #endregion

    #region HttpDelete

    [HttpDelete("delete/{id:long}")]
    public IActionResult Delete(long id)
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
