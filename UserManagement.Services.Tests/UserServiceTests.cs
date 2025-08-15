using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes the service and sets up mock users in the data context.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Calls the method under test to retrieve all users.
        var result = service.GetAll();

        // Assert: Verifies that the service returns the same users from the data context.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void GetById_WhenUserExists_MustReturnSameUser()
    {
        // Arrange: Initializes the service and sets up mock users in the data context.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Calls the method under test to retrieve a user by ID.
        var result = service.GetById(users.First().Id);

        // Assert: Verifies that the returned user matches the expected user from the data context.
        result.Should().BeSameAs(users.First());
    }

    [Fact]
    public void FilterByActive_WhenCalled_MustReturnOnlyActiveUsers()
    {
        // Arrange: Initializes the service and sets up only active users in the data context.
        var service = CreateService();
        var users = SetupUsers(isActive: true);

        // Act: Calls the method under test to filter active users.
        var result = service.FilterByActive(true);

        // Assert: Verifies that all returned users are active.
        result.All(x => x.IsActive).Should().BeTrue();
    }

    [Fact]
    public void Update_WhenCalled_MustCallDataContextUpdate()
    {
        // Arrange: Initializes the service and sets up mock users.
        var service = CreateService();
        var user = SetupUsers().First();

        // Act: Calls the method under test to update the user.
        service.Update(user);

        // Assert: Verifies that the data context's Update method was called once with the correct user.
        _dataContext.Verify(d => d.Update(user), Times.Once);
    }

    [Fact]
    public void Delete_WhenCalled_MustCallDataContextDelete()
    {
        // Arrange: Initializes the service and sets up mock users.
        var service = CreateService();
        var user = SetupUsers().First();

        // Act: Calls the method under test to delete the user.
        service.Delete(user);

        // Assert: Verifies that the data context's Delete method was called once with the correct user.
        _dataContext.Verify(d => d.Delete(user), Times.Once);
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

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        _dataContext
            .Setup(d => d.Find<User>(id))
            .Returns(users.First(x => x.Id == id));

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
