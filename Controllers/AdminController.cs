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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // User Management

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                };
                userViewModels.Add(userViewModel);
            }

            return View("/Views/Admin/Users/Index.cshtml", userViewModels);
        }

        // Edit user info and roles (Get)
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    model.Roles = roles.ToList();
                }
            }

            return View("/Views/Admin/Users/AddUser.cshtml", model);
        }

        // Edit user info and roles (Post)
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid) // Is user input valid?
            {
                var user = await _userManager.FindByIdAsync(model.Id); // Retrieve user with specified ID

                if (user != null) // If user is found, set user Email and Username to input values
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    var result = await _userManager.UpdateAsync(user); // Update user info

                    if (result.Succeeded) // If update was successful, set user roles to selected input values
                    {
                        var selectedRoles = model.SelectedRoles ?? new string[] { };

                        var roles = await _userManager.GetRolesAsync(user); // Get current roles of user

                        result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(roles)); // Add user input roles with exception of current roles

                        if (!result.Succeeded) // If update is not successful, return error message and page view
                        {
                            ModelState.AddModelError("", "Failed to add selected roles to user.");
                            return View(model);
                        }

                        result = await _userManager.RemoveFromRolesAsync(user, roles.Except(selectedRoles)); // Keep only the selected roles from user input

                        if (!result.Succeeded) // If role edit is not successful, return error message and page view
                        {
                            ModelState.AddModelError("", "Failed to remove deselected roles from user.");
                            return View(model);
                        }

                        return RedirectToAction("Index", "Admin"); // If all edits are successful, redirect to Index view of current controller
                    }
                }

                ModelState.AddModelError("", "User not found."); // If user is not found, return error message and page view
            }

            return View("/Views/Admin/Users/Index.cshtml", model);
        }

        // Delete user
        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id); // Retrieve user with specified ID

            if (user != null) // If user is found, delete user
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded) // if deletion was successful, redirect back to Index view, else return error message
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", "Failed to delete user.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found"); // If user is not found, return error message
            }

            return RedirectToAction("Index", "Admin"); // Return Index view for list of users
        }
    }
}
