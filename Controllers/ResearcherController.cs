using ByuEgyptSite.Controllers;
using ByuEgyptSite.Data;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace ByuEgyptSite.Controllers
{
    //[Authorize(Roles = "Administrator, Researcher")]

    // Constructor
    public class ResearcherController : Controller
    {
        private readonly ILogger<ResearcherController> _logger;
        private ApplicationDbContext _burialContext { get; set; }
        public ResearcherController(ILogger<ResearcherController> logger, ApplicationDbContext ac)
        {
            _logger = logger;
            _burialContext = ac;
        }

        // Return Supervised Analysis view
        public IActionResult SupervisedAnalysis()
        {
            return View("/Views/Analysis/SupervisedAnalysis.cshtml");
        }

        // Return Unsupervised Analysis view
        public IActionResult UnsupervisedAnalysis()
        {
            return View("/Views/Analysis/UnsupervisedAnalysis.cshtml");
        }

        // Adding and editing records
        
        [HttpGet]
        // Return the AddRecord view
        public IActionResult AddRecord()
        {
            return View("/Views/Admin/Records/AddRecord.cshtml");
        }

        [HttpPost]

        // Given a valid record entry, add the entry to the database _burialContext
        // and return the AddRecord Confirmation view along with the entry name
        public IActionResult AddRecord(Burial bur)
        {
            if (ModelState.IsValid) // If entry is valid, add the object and return confirmation view
            {
                _burialContext.Add(bur);
                _burialContext.SaveChanges();

                return View("/Views/Admin/Records/AddRecordConfirmation.cshtml", bur);
            }
            else // If entry is invalid, return AddRecord view with record to be added
            {
                return View(bur);
            }
        }

        // Function to edit table row (Get)
        [HttpGet]
        public IActionResult Edit(long burialId)
        {
            var burial = _burialContext.Burials.Single(x => x.id == burialId);

            return View("/Views/Admin/Records/AddRecord.cshtml", burial); // return the AddRecord view along with the record for the single entry
        }

        // Function to edit table row (Post)
        [HttpPost]
        public IActionResult Edit(Burial bur)
        {
            _burialContext.Update(bur);
            _burialContext.SaveChanges();

            return RedirectToAction("/Views/Home/BurialSummary.cshtml"); // update record and return to home burial list
        }

        // Function to delete table row (Post)
        [HttpPost]
        public IActionResult DeleteRecordConfirmed(long burialid)
        {
            var record = _burialContext.Burials.Single(x => x.id == burialid);
            _burialContext.Burials.Remove(record);
            _burialContext.SaveChanges();

            return RedirectToAction("BurialSummary", "home"); // delete record and return confirmation view
        }
    }
}