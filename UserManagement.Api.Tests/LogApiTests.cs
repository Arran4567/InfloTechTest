using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Controllers;
using UserManagement.Data.Enums;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Data.Tests;

public class LogApiTests
{
    [Fact]
    public void List_WhenServiceReturnsLogs_ReturnsAllLogs()
    {
        var controller = CreateController();
        var logs = SetupLogs();

        var result = controller.List() as OkObjectResult;

        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(logs);
    }

    [Fact]
    public void List_WithFilterParameter_CallsFilterByType_ReturnsFilteredLogs()
    {
        var controller = CreateController();
        var logs = SetupLogs(LogType.View);
        _logService.Setup(s => s.FilterByType(LogType.View)).Returns(logs);

        var result = controller.List(LogType.View) as OkObjectResult;

        _logService.Verify(s => s.FilterByType(LogType.View), Times.Once);
        result!.Value.Should().BeEquivalentTo(logs);
    }

    [Fact]
    public void Detail_WhenLogExists_ReturnsLog()
    {
        var controller = CreateController();
        var log = SetupLogs().First();
        _logService.Setup(s => s.GetById(log.Id)).Returns(log);

        var result = controller.Detail(log.Id) as OkObjectResult;

        result.Should().NotBeNull();
        result!.Value.Should().BeEquivalentTo(log);
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

    private readonly Mock<ILogService> _logService = new();
    private LogsController CreateController() => new(_logService.Object);
}
