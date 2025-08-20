using System.ComponentModel.DataAnnotations;

namespace UserManagement.UI.Models;

public class User
{
    public string Id { get; set; } = default!;
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; } = default!;
    public ICollection<Log> Logs { get; set; } = new List<Log>();
}
public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public required string Id { get; set; }
    public required string Forename { get; set; }
    public required string Surname { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; }
    public LogListViewModel Logs { get; set; } = default!;
}

public static class UserExtensions
{
    public static UserListItemViewModel ToListItemViewModel(this User user) => new UserListItemViewModel
    {
        Id = user.Id,
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        IsActive = user.IsActive,
        DateOfBirth = user.DateOfBirth,
        Logs = user.Logs.ToListViewModel()
    };
}
