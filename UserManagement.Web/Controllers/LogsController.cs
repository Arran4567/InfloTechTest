using System.Linq;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService userService) => _logService = userService;

    #region Public Methods

    #region HttpGet

    [HttpGet("")]
    [HttpGet("list")]
    public ViewResult List(LogType? filter = null)
    {
        var logs = filter.HasValue ? _logService.FilterByType(filter.Value) : _logService.GetAll();
        var model = LogListToViewModel(logs);
        return View(model);
    }

    [HttpGet("Detail")]
    public ViewResult Detail(long id)
    {
        var log = _logService.GetById(id);
        if(log == null)
        {
            return View("Error");
        }
        var model = LogToViewModel(log);
        return View(model);
    }

    #endregion

    #endregion


    #region Private Methods

    private LogListViewModel LogListToViewModel(IEnumerable<Log> logs)
    {
        var items = logs.Select(l => LogToViewModel(l)).ToList();
        return new LogListViewModel { Items = items };
    }

    private LogListItemViewModel LogToViewModel(Log log)
    {
        return new LogListItemViewModel
        {
            Id = log.Id,
            Type = log.Type,
            Description = log.Description,
            DateTime = log.DateTime
        };
    }

    #endregion
}
