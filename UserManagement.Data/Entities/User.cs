using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Models;

public class User : IdentityUser
{
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; } = default!;
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
