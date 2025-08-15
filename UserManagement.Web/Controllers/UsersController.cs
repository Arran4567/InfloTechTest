using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet("")]
    [HttpGet("List")]
    public ViewResult List(bool? filter = null)
    {
        var users = _userService.GetAll();
        var model = UserListToViewModel(users);

        return View(model);
    }

    [HttpGet("filter/{active:bool}")]
    public ViewResult Filter(bool active)
    {
        var users = _userService.FilterByActive(active);
        var model = UserListToViewModel(users);

        return View("List", model);
    }

    [HttpGet("detail/{id:long}")]
    public ViewResult Detail(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return View("Error");
        }

        var model = UserToViewModel(user);
        return View(model);
    }

    [HttpGet("edit/{id:long}")]
    public ViewResult Edit(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return View("Error");
        }

        var model = UserToViewModel(user);
        return View(model);
    }

    [HttpGet("delete/{id:long}")]
    public IActionResult Delete(long id)
    {
        var entityToDelete = _userService.GetById(id);

        if (entityToDelete == null)
        {
            return View("Error");
        }

        _userService.Delete(entityToDelete);
        return RedirectToAction("List");
    }

    [HttpPost("edit/{id:long}")]
    public IActionResult Edit(User? model)
    {
        if (model == null)
        {
            return View("Error");
        }

        _userService.Update(model);
        return RedirectToAction("List");
    }

    private UserListViewModel UserListToViewModel(IEnumerable<User> users)
    {
        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        return new UserListViewModel
        {
            Items = items.ToList()
        };
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
            IsActive = user.IsActive
        };
    }
}
