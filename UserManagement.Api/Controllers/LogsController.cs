using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Models.Logs;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Api.Controllers;

[Route("api/logs")]
[ApiController]
[Authorize]
public class LogsController : ControllerBase
{
    private readonly ILogService _logService;
    public LogsController(ILogService userService) => _logService = userService;

    #region Public Methods

    #region HttpGet

    [HttpGet("")]
    [HttpGet("list")]
    public IActionResult List(LogType? filter = null)
    {
        var model = filter.HasValue ? _logService.FilterByType(filter.Value) : _logService.GetAll();
        return Ok(LogListToViewModel(model));
    }

    [HttpGet("Detail")]
    public IActionResult Detail(long id)
    {
        var model = _logService.GetById(id);
        if (model == null)
        {
            return BadRequest("Log not found");
        }

        return Ok(LogToViewModel(model));
    }

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

    #endregion
}
