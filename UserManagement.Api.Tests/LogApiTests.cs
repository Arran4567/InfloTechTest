using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Controllers;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Api.Models.Logs;

namespace UserManagement.Data.Tests;

public class LogApiTests
{
    private readonly Mock<ILogService> _logService = new();
    private LogsController CreateController() => new(_logService.Object);

    [Fact]
    public void List_WhenServiceReturnsLogs_ReturnsAllLogs()
    {
        var controller = CreateController();
        var logs = SetupLogs();

        var expected = logs.Select(l => new LogListItemViewModel
        {
            Id = l.Id,
            Type = l.Type,
            Description = l.Description,
            DateTime = l.DateTime
        }).ToList();

        var result = controller.List() as OkObjectResult;

        result.Should().NotBeNull();
        var vm = result!.Value as LogListViewModel;
        vm.Should().NotBeNull();
        vm!.Items.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void List_WithFilterParameter_CallsFilterByType_ReturnsFilteredLogs()
    {
        var controller = CreateController();
        var logs = SetupLogs(LogType.View);
        _logService.Setup(s => s.FilterByType(LogType.View)).Returns(logs);

        var expected = logs.Select(l => new LogListItemViewModel
        {
            Id = l.Id,
            Type = l.Type,
            Description = l.Description,
            DateTime = l.DateTime
        }).ToList();

        var result = controller.List(LogType.View) as OkObjectResult;

        _logService.Verify(s => s.FilterByType(LogType.View), Times.Once);

        var vm = result!.Value as LogListViewModel;
        vm.Should().NotBeNull();
        vm!.Items.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Detail_WhenLogExists_ReturnsLog()
    {
        var controller = CreateController();
        var log = SetupLogs().First();
        _logService.Setup(s => s.GetById(log.Id)).Returns(log);

        var expected = new LogListItemViewModel
        {
            Id = log.Id,
            Type = log.Type,
            Description = log.Description,
            DateTime = log.DateTime
        };

        var result = controller.Detail(log.Id) as OkObjectResult;

        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Detail_WhenLogDoesNotExist_ReturnsBadRequest()
    {
        var controller = CreateController();
        _logService.Setup(s => s.GetById(It.IsAny<long>())).Returns((Log?)null);

        var result = controller.Detail(1) as BadRequestObjectResult;

        result.Should().NotBeNull();
        result!.Value.Should().Be("Log not found");
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

        _logService.Setup(s => s.GetAll()).Returns(logs);
        _logService.Setup(d => d.GetById(id)).Returns(logs.First(x => x.Id == id));

        return logs;
    }
}
