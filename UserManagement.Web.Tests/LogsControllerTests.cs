using System;
using System.Linq;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Controllers;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Data.Tests;

public class LogsControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsLogs_ModelMustContainLogs()
    {
        var controller = CreateController();
        var logs = SetupLogs();

        var result = controller.List();

        result.Model
            .Should().BeOfType<LogListViewModel>()
            .Which.Items.Should().BeEquivalentTo(
                logs.Select(l => new { l.Id, l.Type, l.Description, l.DateTime }));
    }

    [Fact]
    public void List_WithFilterParameter_CallsFilterByType()
    {
        var controller = CreateController();
        var logs = SetupLogs(LogType.View);
        _logService.Setup(s => s.FilterByType(LogType.View)).Returns(logs);

        var result = controller.List(LogType.View);

        _logService.Verify(s => s.FilterByType(LogType.View), Times.Once);
        result.Model.Should().BeOfType<LogListViewModel>();
    }

    [Fact]
    public void Detail_WhenLogExists_ModelMustContainLog()
    {
        var controller = CreateController();
        var log = SetupLogs().First();
        _logService.Setup(s => s.GetById(log.Id)).Returns(log);

        var result = controller.Detail(log.Id);

        result.Model
            .Should().BeOfType<LogListItemViewModel>()
            .Which.Id.Should().Be(log.Id);
    }

    [Fact]
    public void Detail_WhenLogDoesNotExist_ReturnsErrorView()
    {
        var controller = CreateController();
        _logService.Setup(s => s.GetById(It.IsAny<long>())).Returns((Log?)null);

        var result = controller.Detail(1);

        result.ViewName.Should().Be("Error");
    }

    private IQueryable<Log> SetupLogs(LogType type = LogType.View, string description = "Test log", long id = 1)
    {
        var logs = new[]
        {
            new Log
            {
                Id = id,
                Type = type,
                Description = description,
                DateTime = DateTime.UtcNow
            }
        }.AsQueryable();

        _logService
            .Setup(s => s.GetAll())
            .Returns(logs);

        _logService
            .Setup(d => d.GetById(id))
            .Returns(logs.First(x => x.Id == id));

        return logs;
    }

    private readonly Mock<ILogService> _logService = new();
    private LogsController CreateController() => new(_logService.Object);
}
