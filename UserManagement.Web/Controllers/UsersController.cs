using System.Linq;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    #region Public Methods

    #region HttpGet

    [HttpGet("")]
    [HttpGet("list")]
    public ViewResult List(bool? filter = null)
    {
        var users = filter.HasValue ? _userService.FilterByActive(filter.Value) : _userService.GetAll();
        var model = UserListToViewModel(users);

        return View(model);
    }

    [HttpGet("create")]
    public ViewResult Create()
    {
        return View();
    }

    [HttpGet("detail/{id}")]
    public ViewResult Detail(string id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return View("Error");
        }

        _userService.AddLog(user.Id, LogType.View);
        var model = UserToViewModel(user);
        return View(model);
    }

    [HttpGet("edit/{id}")]
    public ViewResult Edit(string id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return View("Error");
        }

        var model = UserToViewModel(user);
        return View(model);
    }

    [HttpGet("delete/{id}")]
    public IActionResult Delete(string id)
    {
        var entityToDelete = _userService.GetById(id);

        if (entityToDelete == null)
        {
            return View("Error");
        }

        _userService.AddLog(entityToDelete.Id, LogType.Delete);
        _userService.Delete(entityToDelete);
        return RedirectToAction("List");
    }

    #endregion

    #region HttpPost

    [HttpPost("Create")]
    public IActionResult Create(UserListItemViewModel? model)
    {
        if (model == null)
        {
            return View("Error");
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
        _userService.AddLog(user.Id, LogType.Create);
        return RedirectToAction("List");
    }

    [HttpPost("edit/{id}")]
    public IActionResult Edit(UserListItemViewModel? model)
    {
        if (model == null)
        {
            return View("Error");
        }

        var user = _userService.GetById(model.Id);
        user!.Forename = model.Forename;
        user!.Surname = model.Surname;
        user!.Email = model.Email;
        user!.DateOfBirth = model.DateOfBirth;
        user!.IsActive = model.IsActive;


        _userService.Update(user);
        _userService.AddLog(user.Id, LogType.Update);
        return RedirectToAction("List");
    }
    #endregion

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
}
