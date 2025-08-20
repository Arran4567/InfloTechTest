using System.ComponentModel.DataAnnotations;
using UserManagement.Api.Models.Logs;

namespace UserManagement.Api.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public string? Id { get; set; }
    public required string Forename { get; set; }
    public required string Surname { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; }
    public LogListViewModel Logs { get; set; } = default!;
}
