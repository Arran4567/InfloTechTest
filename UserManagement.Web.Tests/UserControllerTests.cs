using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Create controller and setup mock users
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Call the List action
        var result = controller.List();

        // Assert: The model should be of type UserListViewModel and contain the expected users
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users, opts => opts
                .ExcludingMissingMembers()
                .Excluding(u => u.Logs));
    }

    [Fact]
    public void Create_Get_ReturnsView()
    {
        var controller = CreateController();

        // Act
        var result = controller.Create();

        // Assert: Simply returns the Create view
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void Create_Post_WhenModelIsValid_CallsCreateAndRedirects()
    {
        var controller = CreateController();
        var user = SetupUsers().First();

        var result = controller.Create(UserToViewModel(user)) as RedirectToActionResult;

        _userService.Verify(s => s.Create(It.IsAny<User>()), Times.Once);
        result!.ActionName.Should().Be("List");
    }

    [Fact]
    public void Create_Post_WhenModelIsNull_ReturnsErrorView()
    {
        var controller = CreateController();

        var result = controller.Create(null);

        result.Should().BeOfType<ViewResult>().Which.ViewName.Should().Be("Error");
    }

    [Fact]
    public void List_WithOptionalFilterParameter_CallsGetAll()
    {
        var controller = CreateController();
        SetupUsers();

        var result = controller.List(filter: true);

        _userService.Verify(s => s.FilterByActive(true), Times.Once);
        result.Model.Should().BeOfType<UserListViewModel>();
    }

    [Fact]
    public void Detail_WhenUserExists_ModelMustContainUser()
    {
        // Arrange: Create controller and setup a single user
        var controller = CreateController();
        var user = SetupUsers().First();
        _userService.Setup(s => s.GetById(user.Id)).Returns(user);

        // Act: Call the Detail action
        var result = controller.Detail(user.Id);

        // Assert: The model should be of type UserListItemViewModel and have the correct user ID
        result.Model
            .Should().BeOfType<UserListItemViewModel>()
            .Which.Id.Should().Be(user.Id);
    }

    [Fact]
    public void Detail_WhenUserDoesNotExist_ReturnsErrorView()
    {
        // Arrange: Create controller and setup GetById to return null
        var controller = CreateController();
        _userService.Setup(s => s.GetById(It.IsAny<string>())).Returns((User?)null);

        // Act: Call the Detail action
        var result = controller.Detail("a");

        // Assert: The view returned should be "Error"
        result.ViewName.Should().Be("Error");
    }

    [Fact]
    public void Edit_Get_WhenUserExists_ModelMustContainUser()
    {
        // Arrange: Create controller and setup a single user
        var controller = CreateController();
        var user = SetupUsers().First();
        _userService.Setup(s => s.GetById(user.Id)).Returns(user);

        // Act: Call the GET Edit action
        var result = controller.Edit(user.Id);

        // Assert: The model should be of type UserListItemViewModel and have the correct user ID
        result.Model
            .Should().BeOfType<UserListItemViewModel>()
            .Which.Id.Should().Be(user.Id);
    }

    [Fact]
    public void Edit_Get_WhenUserDoesNotExist_ReturnsErrorView()
    {
        // Arrange: Create controller and setup GetById to return null
        var controller = CreateController();
        _userService.Setup(s => s.GetById(It.IsAny<string>())).Returns((User?)null);

        // Act: Call the GET Edit action
        var result = controller.Edit("a");

        // Assert: The view returned should be "Error"
        result.ViewName.Should().Be("Error");
    }

    [Fact]
    public void Edit_Post_WhenModelIsValid_CallsUpdateAndRedirects()
    {
        // Arrange: Create controller and setup a valid user
        var controller = CreateController();
        var user = SetupUsers().First();

        // Act: Call the POST Edit action
        var result = controller.Edit(UserToViewModel(user)) as RedirectToActionResult;

        // Assert: Update should be called once and redirect to "List"
        _userService.Verify(s => s.Update(user), Times.Once);
        result!.ActionName.Should().Be("List");
    }

    [Fact]
    public void Edit_Post_WhenModelIsNull_ReturnsErrorView()
    {
        // Arrange: Create controller
        var controller = CreateController();

        // Act: Call the POST Edit action with an empty User (simulates null model scenario)
        var result = controller.Edit((UserListItemViewModel?)null);

        // Assert: The view returned should be "Error"
        result.Should().BeOfType<ViewResult>().Which.ViewName.Should().Be("Error");
    }

    [Fact]
    public void Delete_WhenUserExists_CallsDeleteAndRedirects()
    {
        // Arrange: Create controller and setup a single user
        var controller = CreateController();
        var user = SetupUsers().First();
        _userService.Setup(s => s.GetById(user.Id)).Returns(user);

        // Act: Call the Delete action
        var result = controller.Delete(user.Id) as RedirectToActionResult;

        // Assert: Delete should be called once and redirect to "List"
        _userService.Verify(s => s.Delete(user), Times.Once);
        result!.ActionName.Should().Be("List");
    }

    [Fact]
    public void Delete_WhenUserDoesNotExist_ReturnsErrorView()
    {
        // Arrange: Create controller and setup GetById to return null
        var controller = CreateController();
        _userService.Setup(s => s.GetById(It.IsAny<string>())).Returns((User?)null);

        // Act: Call the Delete action
        var result = controller.Delete("a");

        // Assert: The view returned should be "Error"
        result.Should().BeOfType<ViewResult>().Which.ViewName.Should().Be("Error");
    }

    private IQueryable<User> SetupUsers(string id = "3c6856e6-36a9-4209-8db3-8be6792b5e24", string forename = "Johnny", string surname = "User", string email = "juser@example.com", DateTime? dateOfBirth = null, bool isActive = true)
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

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        _userService
            .Setup(d => d.GetById(id))
            .Returns(users.First(x => x.Id == id));

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
