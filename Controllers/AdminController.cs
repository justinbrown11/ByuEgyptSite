using ByuEgyptSite.Data;
using Microsoft.AspNetCore.Mvc;
using ByuEgyptSite.Models;

namespace ByuEgyptSite.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _burialContext { get; set; }

        // Constructor
        public AdminController(ApplicationDbContext ac)
        {
            _burialContext = ac;
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
    }
}
