using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BestStore.Models;

namespace WebApp1.Controllers
{
    [Authorize(Roles = "Admin")]  // Restrict access to Admins only
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Display users and their roles
        [HttpGet]
        public async Task<IActionResult> ManageRoles()
        {
            var users = _userManager.Users.ToList();
            var userRoleViewModelList = new List<UserRole>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var currentRole = roles.FirstOrDefault();

                var availableRoles = _roleManager.Roles.Select(r => r.Name).ToList();

                userRoleViewModelList.Add(new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    CurrentRole = currentRole ?? "No Role Assigned",
                    AvailableRoles = availableRoles
                });
            }

            return View(userRoleViewModelList);
        }

        // Assign a role to a user
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                // Remove existing roles
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            // Assign the new role
            await _userManager.AddToRoleAsync(user, roleName);

            return RedirectToAction("ManageRoles");
        }
    }
}
