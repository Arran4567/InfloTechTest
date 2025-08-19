using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Enums;
    using UserManagement.Services.Domain.Interfaces;

    namespace UserManagement.Api.Controllers;

    [Route("api/logs")]
    [ApiController]
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
            return Ok(model);
        }

        [HttpGet("Detail")]
        public IActionResult Detail(long id)
        {
            var model = _logService.GetById(id);
            if (model == null)
            {
                return BadRequest("Log not found");
            }

            return Ok(model);
        }

        #endregion

        #endregion
    }
