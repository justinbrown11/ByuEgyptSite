using ByuEgyptSite.Data;
using Microsoft.AspNetCore.Mvc;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ByuEgyptSite.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        // User Management

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            return View("/Views/Admin/Users/Index.cshtml", users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = null
            };

            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    model.Roles = roles.ToList();
                }
            }

            return View(model);
        }

        // Edit user info and roles
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid) // Is user input valid?
            {
                var user = await userManager.FindByIdAsync(model.Id); // Retrieve user with specified ID

                if (user != null) // If user is found, set user Email and Username to input values
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    var result = await userManager.UpdateAsync(user); // Update user info

                    if (result.Succeeded) // If update was successful, set user roles to selected input values
                    {
                        var selectedRoles = model.SelectedRoles ?? new string[] { };

                        var roles = await userManager.GetRolesAsync(user); // Get current roles of user

                        result = await userManager.AddToRolesAsync(user, selectedRoles.Except(roles)); // Add user input roles with exception of current roles

                        if (!result.Succeeded) // If update is not successful, return error message and page view
                        {
                            ModelState.AddModelError("", "Failed to add selected roles to user.");
                            return View(model);
                        }

                        result = await userManager.RemoveFromRolesAsync(user, roles.Except(selectedRoles)); // Keep only the selected roles from user input

                        if (!result.Succeeded) // If role edit is not successful, return error message and page view
                        {
                            ModelState.AddModelError("", "Failed to remove deselected roles from user.");
                            return View(model);
                        }

                        return RedirectToAction("/Views/Admin/Users/Index.cshtml"); // If all edits are successful, redirect to Index view of current controller
                    }
                }

                ModelState.AddModelError("", "User not found."); // If user is not found, return error message and page view
            }

            return View(model);
        }

        // Delete user
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id); // Retrieve user with specified ID

            if (user != null) // If user is found, delete user
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded) // if deletion was successful, redirect back to Index view, else return error message
                {
                    return RedirectToAction("/Views/Admin/Users/Index.cshtml");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete user.");
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found."); // If user is not found, return error message
            }

            return View("/Views/Admin/Users/Index.cshtml", userManager.Users); // Return Index view for list of users
        }
    }
}
