using System.ComponentModel.DataAnnotations;

namespace UserManagement.Api.Models.Identity;

public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}

