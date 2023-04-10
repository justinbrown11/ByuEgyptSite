using ByuEgyptSite.Data;
using Microsoft.AspNetCore.Mvc;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ByuEgyptSite.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private ApplicationDbContext _burialContext { get; set; }

        // Constructor
        public AdminController(ApplicationDbContext ac, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _burialContext = ac;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]

        // Return the AddRecord view
        public IActionResult AddRecord()
        {
            return View("Admin/Records/AddRecord");
        }

        [HttpPost]

        // Given a valid record entry, add the entry to the database _recordContext
        // and return the AddRecord Confirmation view along with the entry name
        public IActionResult AddRecord(Burial bur)
        {
            if (ModelState.IsValid) // If entry is valid
            {
                _burialContext.Add(bur);
                _burialContext.SaveChanges();

                return View("AddRecordConfirmation", bur);
            }
            else // If entry is invalid
            {
                return View(bur);
            }
        }

        // Function to edit table row (Get)
        [HttpGet]
        public IActionResult Edit (int burialid)
        {
            var burial = _burialContext.Burials.Single(x => x.id == burialid);

            return View("AddRecord", burial); // return the "Enter Movie" page view algon with the record for the single entry
        }

        // Function to edit table row (Post)
        [HttpPost]

        public IActionResult Edit (Burial bur)
        {
            _burialContext.Update(bur);
            _burialContext.SaveChanges();

            return RedirectToAction("Home/BurialSummary");
        }

        // Function to delete table row (Get)
        [HttpGet]
        public IActionResult DeleteRecord (int burialid) 
        {
            var record = _burialContext.Burials.Single(x => x.id == burialid);

            return View(record);
        }

        // THESE ARE THE USER ADMIN STUFF, YOU'LL NEED TO CHANGE THE ACt

        //public async Task<IActionResult> Index()
        //{
        //    var users = await userManager.Users.ToListAsync();
        //    return View(users);
        //}

        //public async Task<IActionResult> Edit(string id)
        //{
        //    var user = await userManager.FindByIdAsync(id);
        //    var roles = roleManager.Roles.ToList();

        //    var model = new EditUserViewModel
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        UserName = user.UserName,
        //        Roles = roles
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(EditUserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByIdAsync(model.Id);

        //        if (user != null)
        //        {
        //            user.Email = model.Email;
        //            user.UserName = model.UserName;

        //            var result = await userManager.UpdateAsync(user);

        //            if (result.Succeeded)
        //            {
        //                var selectedRoles = model.SelectedRoles ?? new string[] { };
        //                var roles = await userManager.GetRolesAsync(user);

        //                result = await userManager.AddToRolesAsync(user, selectedRoles.Except(roles));

        //                if (!result.Succeeded)
        //                {
        //                    ModelState.AddModelError("", "Failed to add selected roles to user.");
        //                    return View(model);
        //                }

        //                result = await userManager.RemoveFromRolesAsync(user, roles.Except(selectedRoles));

        //                if (!result.Succeeded)
        //                {
        //                    ModelState.AddModelError("", "Failed to remove deselected roles from user.");
        //                    return View(model);
        //                }

        //                return RedirectToAction("Index");
        //            }
        //        }

        //        ModelState.AddModelError("", "User not found.");
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var user = await userManager.FindByIdAsync(id);

        //    if (user != null)
        //    {
        //        var result = await userManager.DeleteAsync(user);

        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Failed to delete user.");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "User not found.");
        //    }

        //    return View("Index", userManager.Users);
        //}
    }
}
