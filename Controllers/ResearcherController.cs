using ByuEgyptSite.Controllers;
using ByuEgyptSite.Data;
using ByuEgyptSite.MLModel;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Text.Json;


namespace ByuEgyptSite.Controllers
{
    //[Authorize(Roles = "Administrator, Researcher")]
    public class ResearcherController : Controller
    {
        private readonly ILogger<ResearcherController> _logger;
        private ApplicationDbContext _burialContext { get; set; } // Add db context

        // Constructor
        public ResearcherController(ILogger<ResearcherController> logger, ApplicationDbContext ac)
        {
            _logger = logger;
            _burialContext = ac;
        }

        // Return Supervised Analysis view
        [HttpGet]
        public IActionResult SupervisedAnalysis()
        {
            return View("/Views/Researcher/SupervisedAnalysis.cshtml", new UserData());
        }

        // Post form destination for supervised analysis view
        [HttpPost]
        public IActionResult SupervisedAnalysis(UserData data)
        {
            return RedirectToAction("AlterData","Researcher", data);
        }

        [HttpPost]
        public IActionResult AlterData(UserData data)
        {
            var temp = data;
            var json = JsonConvert.SerializeObject(temp);
            TempData["UserInput"] = json;
            //var json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return RedirectToAction("Score","Inference");
        }

        [HttpGet]
        public IActionResult SendPrediction()
        {
            string predictionJson = TempData["prediction"] as string;

            ViewBag.Prediction = predictionJson;

            return View("/Views/Researcher/SupervisedAnalysis.cshtml");
        }

        [HttpGet]
        public IActionResult UnsupervisedAnalysis()
        {
            return View("/Views/Researcher/UnsupervisedAnalysis.cshtml");
        }
        
        // The Add New Record View
        [HttpGet]
        public IActionResult AddRecord()
        {
            // For conditionals in view
            ViewData["Type"] = "add";

            return View("/Views/Admin/Records/AddEditRecord.cshtml");
        }

        // Either adds/edits a record
        [HttpPost]
        public IActionResult AddEditRecord(Burial b)
        {
            if (ModelState.IsValid) // If entry is valid, add the object and return confirmation view
            {
                // Generate random 64 bit id
                byte[] bytes = Guid.NewGuid().ToByteArray();
                long randomId = BitConverter.ToInt64(bytes, 0);

                // Simply to hold for later use
                var id = b.id;

                // If id is passed, keep it, if not, use the randomly generated one
                b.id = b.id != 0 ? b.id : randomId;

                // If burialmainid was passed, use it, if not, generate it
                b.burialmainid = b.burialmainid != null ? b.burialmainid : $"{b.squarenorthsouth}{b.northsouth}{b.squareeastwest}{b.eastwest}{b.area}{b.burialnumber}";

                // Adding a new record
                if (id == 0)
                {
                    _burialContext.Burials.Add(b);
                }

                // Editing an existing record
                else
                {
                    _burialContext.Update(b);
                }

                _burialContext.SaveChanges(); // save changes

                // Redirect to burial record list
                return RedirectToAction("BurialSummary", "Home");
            }
            else // If entry is invalid, return View with issues
            {
                return View("/Views/Admin/Records/AddEditRecord.cshtml", b);
            }
        }

        // The Edit a record view (form)
        [HttpGet]
        public IActionResult Edit(long burialId)
        {
            var burial = _burialContext.Burials.Single(x => x.id == burialId);

            // For conditionals in view
            ViewData["Type"] = "edit";

            return View("/Views/Admin/Records/AddEditRecord.cshtml", burial); // return the AddRecord view along with the record for the single entry
        }

        // Deletes a record
        [HttpPost]
        public IActionResult Delete(long burialid)
        {
            var record = _burialContext.Burials.Single(x => x.id == burialid);
            _burialContext.Burials.Remove(record);
            _burialContext.SaveChanges();

            // Redirect back to burial record list view
            return RedirectToAction("BurialSummary", "Home");
        }
    }
}