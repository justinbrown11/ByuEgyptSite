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
            ViewData["Type"] = "add";

            return View("/Views/Admin/Records/AddEditRecord.cshtml");
        }

        [HttpPost]

        // Given a valid record entry, add the entry to the database _burialContext
        // and return a view
        public IActionResult AddEditRecord(Burial b)
        {
            if (ModelState.IsValid) // If entry is valid, add the object and return confirmation view
            {
                byte[] bytes = Guid.NewGuid().ToByteArray();
                long randomId = BitConverter.ToInt64(bytes, 0);

                var id = b.id;

                b.id = b.id != 0 ? b.id : randomId;
                b.burialmainid = b.burialmainid != null ? b.burialmainid : $"{b.squarenorthsouth}{b.northsouth}{b.squareeastwest}{b.eastwest}{b.area}{b.burialnumber}";

                if (id == 0)
                {
                    _burialContext.Burials.Add(b);
                }
                else
                {
                    _burialContext.Update(b);
                }
                _burialContext.SaveChanges();

                return RedirectToAction("BurialSummary", "Home");
            }
            else // If entry is invalid, return AddRecord view with record to be added
            {
                return View("/Views/Admin/Records/AddEditRecord.cshtml", b);
            }
        }

        // Function to edit
        [HttpGet]
        public IActionResult Edit(long burialId)
        {
            var burial = _burialContext.Burials.Single(x => x.id == burialId);

            ViewData["Type"] = "edit";

            return View("/Views/Admin/Records/AddEditRecord.cshtml", burial); // return the AddRecord view along with the record for the single entry
        }

        // Function to delete table row (Post)
        [HttpPost]
        public IActionResult Delete(long burialid)
        {
            var record = _burialContext.Burials.Single(x => x.id == burialid);
            _burialContext.Burials.Remove(record);
            _burialContext.SaveChanges();

            return RedirectToAction("BurialSummary", "Home"); // delete record and return confirmation view
        }
    }
}