using ByuEgyptSite.Data;
using Microsoft.AspNetCore.Mvc;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Channels;

namespace ByuEgyptSite.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        // Initialize user and role managers
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructer
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Display list of all users
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Retrieve list of all users and create a list of type "UserViewModel"
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            // Iterate through each user in the retrieved list of users
            foreach (var user in users)
            {
                // Retrieve roles and make new UserViewModel for each user
                var roles = await _userManager.GetRolesAsync(user);
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToArray()
                };
                
                // Add each userViewModel to the list userViewModels 
                userViewModels.Add(userViewModel);
            }

            // Return the view along with the newly created list of userViewModels
            return View("/Views/Admin/Users/Index.cshtml", userViewModels);
        }

        // Add a new user view form
        [HttpGet]
        public IActionResult AddUser()
        {
            return View("/Views/Admin/Users/AddUser.cshtml", new UserViewModel());
        }

        // Adds a new user
        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                // Create new user object
                var newUser = new IdentityUser { UserName = user.UserName, Email = user.Email };
                // Grab roles to add
                var rolesToAdd = user.Roles?.Where(role => !string.IsNullOrWhiteSpace(role)).ToArray() ?? new string[0];

                // Create the user
                var result = await _userManager.CreateAsync(newUser);

                // If success
                if (result.Succeeded)
                {
                    // Add roles to the new user
                    if (rolesToAdd.Any())
                    {
                        result = await _userManager.AddToRolesAsync(newUser, rolesToAdd);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View("/Views/Admin/Users/AddUser.cshtml");
                        }
                    }

                    return View("/Views/Admin/Users/AddUserConfirmation.cshtml", user);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View("/Views/Admin/Users/AddUser.cshtml", user);
        }

        // Edit user info and roles view form
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            // Retrieve the user from the _userManager by user id
            var user = await _userManager.FindByIdAsync(Id);

            // Create a new UserViewModel object and store user info and roles
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user)).ToArray() // Retrieve user roles as an array, set model property "Roles" equal to value
            };

            if (user != null)
            {
                // Retrieve user roles as an array, set local variable equal to value
                var roles = (await _userManager.GetRolesAsync(user)).ToArray();
                if (roles.Any())
                {
                    model.Roles = roles;
                }
            }

            return View("/Views/Admin/Users/AddUser.cshtml", model);
        }

        // Edits user info and roles
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            // Is user input valid?
            if (ModelState.IsValid)
            {
                // Retrieve user with specified ID
                var user = await _userManager.FindByIdAsync(model.Id);

                // If user is found, set user Email and Username to input values
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    // Update the user's roles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var rolesToRemove = currentRoles.Except(model.Roles).ToArray();
                    var rolesToAdd = model.Roles.Except(currentRoles).ToArray();

                    // Remove roles
                    var result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                    // If removing role(s) was not successful, return error message and page view
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to remove existing role(s) from user.");
                        return View("/Views/Admin/Users/AddUser.cshtml", model);
                    }

                    // Add roles
                    result = await _userManager.AddToRolesAsync(user, rolesToAdd);

                    // If adding role(s) was not successful, return error message and page view
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to add selected role(s) to user.");
                        return View("/Views/Admin/Users/AddUser.cshtml", model);
                    }

                    // Save the changes to the user
                    result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to save changes to user.");
                        return View("/Views/Admin/Users/AddUser.cshtml", model);
                    }
                }

                else
                {
                    // If user is not found, return error message and page view
                    ModelState.AddModelError("", "User not found.");
                    return RedirectToAction("Index", "Admin");
                }
            }

            return RedirectToAction("Index", "Admin");
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

        // Redirect to User Index page
        public IActionResult RedirectToUserIndex()
        {
            return RedirectToAction("Index", "Admin");
        }
    }
}
