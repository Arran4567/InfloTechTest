using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Controllers;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Api.Tests;

public class UserApiTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ReturnsOkWithUsers()
    {
        var controller = CreateController();
        var users = SetupUsers();

        var result = controller.List() as OkObjectResult;

        result!.StatusCode.Should().Be(200);
        var returnedUsers = result.Value as IQueryable<User>;
        returnedUsers.Should().BeEquivalentTo(users, opts => opts.Excluding(u => u.Logs));
    }

    [Fact]
    public void List_WithOptionalFilterParameter_CallsFilterByActive()
    {
        var controller = CreateController();
        var users = SetupUsers();

        var result = controller.List(filter: true) as OkObjectResult;

        _userService.Verify(s => s.FilterByActive(true), Times.Once);

        result!.Value.Should().BeAssignableTo<IEnumerable<User>>()
            .Which.Should().BeEquivalentTo(users, opts => opts.Excluding(u => u.Logs));
    }

    [Fact]
    public void Detail_WhenUserExists_ReturnsOkWithUser()
    {
        var controller = CreateController();
        var user = SetupUsers().First();
        _userService.Setup(s => s.GetById(user.Id)).Returns(user);

        var result = controller.Detail(user.Id) as OkObjectResult;

        result!.StatusCode.Should().Be(200);
        var returnedUser = result.Value as User;
        returnedUser!.Id.Should().Be(user.Id);
    }

    [Fact]
    public void Detail_WhenUserDoesNotExist_ReturnsOkWithErrorMessage()
    {
        var controller = CreateController();
        _userService.Setup(s => s.GetById(It.IsAny<long>())).Returns((User?)null);

        var result = controller.Detail(1) as OkObjectResult;

        result!.StatusCode.Should().Be(200);
        result.Value.Should().Be("User not found.");
    }

    [Fact]
    public void Create_Post_WhenModelIsValid_CreatesUserAndReturnsOk()
    {
        var controller = CreateController();
        var user = SetupUsers().First();

        var result = controller.Create(user) as OkObjectResult;

        _userService.Verify(s => s.Create(user), Times.Once);
        result!.Value.Should().Be(user);
    }

    [Fact]
    public void Create_Post_WhenModelIsInvalid_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Invalid model");

        var result = controller.Create(new User());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Edit_Put_WhenModelIsValid_UpdatesUserAndReturnsOk()
    {
        var controller = CreateController();
        var user = SetupUsers().First();

        var result = controller.Edit(user) as OkObjectResult;

        _userService.Verify(s => s.Update(user), Times.Once);
        result!.Value.Should().Be(user);
    }

    [Fact]
    public void Edit_Put_WhenModelIsInvalid_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Invalid model");

        var result = controller.Edit(new User());

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Delete_WhenUserExists_DeletesUserAndReturnsOk()
    {
        var controller = CreateController();
        var user = SetupUsers().First();
        _userService.Setup(s => s.GetById(user.Id)).Returns(user);

        var result = controller.Delete(user.Id) as OkResult;

        _userService.Verify(s => s.Delete(user), Times.Once);
        result!.StatusCode.Should().Be(200);
    }

    [Fact]
    public void Delete_WhenUserDoesNotExist_ReturnsBadRequest()
    {
        var controller = CreateController();
        _userService.Setup(s => s.GetById(It.IsAny<long>())).Returns((User?)null);

        var result = controller.Delete(1) as BadRequestObjectResult;

        result!.Value.Should().Be("User not found.");
    }

    private IQueryable<User> SetupUsers(long id = 1, string forename = "Johnny", string surname = "User", string email = "juser@example.com", DateOnly? dateOfBirth = null, bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Id = id,
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = dateOfBirth ?? new DateOnly(1972, 04, 15),
                IsActive = isActive
            }
        }.AsQueryable();

        _userService.Setup(s => s.GetAll()).Returns(users);
        _userService.Setup(s => s.FilterByActive(true)).Returns(users.Where(x => x.IsActive));
        _userService.Setup(s => s.FilterByActive(false)).Returns(users.Where(x => !x.IsActive));
        _userService.Setup(d => d.GetById(id)).Returns(users.First(x => x.Id == id));

        return users;
    }
    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
