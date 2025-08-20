using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserManagement.Models;

namespace UserManagement.Data
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }

    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            var users = new[]
            {
                new User { Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new DateTime(1985, 03, 12) },
                new User { Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new DateTime(1972, 11, 05) },
                new User { Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = new DateTime(1990, 07, 21) },
                new User { Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = new DateTime(1998, 02, 17) },
                new User { Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = new DateTime(1980, 09, 30) },
                new User { Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = new DateTime(1995, 06, 04) },
                new User { Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = new DateTime(1988, 12, 19) },
                new User { Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth = new DateTime(1978, 04, 23) },
                new User { Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth = new DateTime(1992, 08, 11) },
                new User { Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth = new DateTime(1983, 05, 29) },
                new User { Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth = new DateTime(1975, 10, 02) },
            };

            foreach (var user in users)
            {
                user.UserName = $"{user.Surname}{user.Forename[0]}";
                if (await _userManager.FindByEmailAsync(user.Email!) is null)
                {
                    var password = $"{user.Surname}123!";
                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
