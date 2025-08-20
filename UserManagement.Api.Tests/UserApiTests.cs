using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Controllers;
using UserManagement.Api.Models.Users;
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
        _userService.Setup(s => s.GetById(It.IsAny<string>())).Returns((User?)null);

        var result = controller.Detail("a") as OkObjectResult;

        result!.StatusCode.Should().Be(200);
        result.Value.Should().Be("User not found.");
    }

    [Fact]
    public void Create_Post_WhenModelIsValid_CreatesUserAndReturnsOk()
    {
        var controller = CreateController();
        var user = new User
        {
            Forename = "Test",
            Surname = "Test",
            DateOfBirth = new DateTime(2000,01,01),
            Email = "Test@Test.com",
            IsActive = true
        };
        var result = controller.Create(UserToViewModel(user)) as OkObjectResult;
        _userService.Verify(s => s.Create(It.IsAny<User>()), Times.Once);
        result!.Value.Should().BeEquivalentTo(UserToViewModel(user), options => options.Excluding(x => x.Id).Excluding(x => x.Logs));
    }

    [Fact]
    public void Create_Post_WhenModelIsInvalid_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Invalid model");

        var result = controller.Create(new UserListItemViewModel { Forename = "", Surname = ""});

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Edit_Put_WhenModelIsValid_UpdatesUserAndReturnsOk()
    {
        var controller = CreateController();
        var user = SetupUsers().First();

        var result = controller.Edit(UserToViewModel(user)) as OkObjectResult;

        _userService.Verify(s => s.Update(user), Times.Once);
        result!.Value.Should().Be(user);
    }

    [Fact]
    public void Edit_Put_WhenModelIsInvalid_ReturnsBadRequest()
    {
        var controller = CreateController();
        controller.ModelState.AddModelError("Error", "Invalid model");

        var result = controller.Edit(new UserListItemViewModel { Forename = "", Surname = "" });

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
        _userService.Setup(s => s.GetById(It.IsAny<string>())).Returns((User?)null);

        var result = controller.Delete("a") as BadRequestObjectResult;

        result!.Value.Should().Be("User not found.");
    }

    private IQueryable<User> SetupUsers(string id = "a", string forename = "Johnny", string surname = "User", string email = "juser@example.com", DateTime? dateOfBirth = null, bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Id = id,
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = dateOfBirth ?? new DateTime(1972, 04, 15),
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

    private UserListItemViewModel UserToViewModel(User user) => new UserListItemViewModel
    {
        Id = user.Id,
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        IsActive = user.IsActive,
    };
}
