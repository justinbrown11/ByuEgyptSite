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
        [Authorize(Roles = "Administrator, Researcher")]
        [HttpGet]
        public IActionResult SupervisedAnalysis()
        {
            return View("/Views/Researcher/SupervisedAnalysis.cshtml", new DataToChange());
        }

        // Post form destination for supervised analysis view
        [Authorize(Roles = "Administrator, Researcher")]
        [HttpPost]
        public IActionResult SupervisedAnalysis(DataToChange data)
        {
            return RedirectToAction("AlterData","Researcher", data);
        }

        [HttpPost]
        public IActionResult AlterData(DataToChange data)
        {
            var userData = new UserData();

            userData.depth = data.depth;
            userData.length = data.length;

            if (data.area == "NE")
            {
                userData.area_NE = 1;
                userData.area_NW = 0;
                userData.area_SE = 0;
                userData.area_SW = 0;
            }
            else if (data.area == "NW")
            {
                userData.area_NE = 0;
                userData.area_NW = 1;
                userData.area_SE = 0;
                userData.area_SW = 0;
            }
            else if (data.area == "SE")
            {
                userData.area_NE = 0;
                userData.area_NW = 0;
                userData.area_SE = 1;
                userData.area_SW = 0;
            }
            else if (data.area == "SW")
            {
                userData.area_NE = 0;
                userData.area_NW = 0;
                userData.area_SE = 0;
                userData.area_SW = 1;
            }
            else
            {
                userData.area_NE = 0;
                userData.area_NW = 0;
                userData.area_SE = 0;
                userData.area_SW = 0;
            }

            if (data.wrapping == "B")
            {
                userData.wrapping_B = 1;
                userData.wrapping_H = 0;
                userData.wrapping_W = 0;
            }
            else if (data.area == "H")
            {
                userData.wrapping_B = 0;
                userData.wrapping_H = 1;
                userData.wrapping_W = 0;
            }
            else if (data.area == "W")
            {
                userData.wrapping_B = 0;
                userData.wrapping_H = 0;
                userData.wrapping_W = 1;
            }
            else
            {
                userData.wrapping_B = 0;
                userData.wrapping_H = 0;
                userData.wrapping_W = 0;
            }

            if (data.samplescollected == "False")
            {
                userData.samplescollected_false = 1;
                userData.samplescollected_true = 0;
            }
            else if (data.samplescollected == "True")
            {
                userData.samplescollected_false = 0;
                userData.samplescollected_true = 1;
            }
            else
            {
                userData.samplescollected_false = 0;
                userData.samplescollected_true = 0;
            }

            if (data.ageatdeath == "A")
            {
                userData.ageatdeath_A = 1;
                userData.ageatdeath_C = 0;
                userData.ageatdeath_I = 0;
                userData.ageatdeath_N = 0;
            }
            else if (data.ageatdeath == "C")
            {
                userData.ageatdeath_A = 0;
                userData.ageatdeath_C = 1;
                userData.ageatdeath_I = 0;
                userData.ageatdeath_N = 0;
            }
            else if (data.ageatdeath == "I")
            {
                userData.ageatdeath_A = 0;
                userData.ageatdeath_C = 0;
                userData.ageatdeath_I = 1;
                userData.ageatdeath_N = 0;
            }
            else if (data.ageatdeath == "N")
            {
                userData.ageatdeath_A = 0;
                userData.ageatdeath_C = 0;
                userData.ageatdeath_I = 0;
                userData.ageatdeath_N = 1;
            }
            else
            {
                userData.ageatdeath_A = 0;
                userData.ageatdeath_C = 0;
                userData.ageatdeath_I = 0;
                userData.ageatdeath_N = 0;
            }

            var temp = userData;
            var json = JsonConvert.SerializeObject(temp);
            TempData["UserInput"] = json;
            return RedirectToAction("Score", "Inference");
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
        [Authorize(Roles = "Administrator, Researcher")]
        public IActionResult AddRecord()
        {
            // For conditionals in view
            ViewData["Type"] = "add";

            return View("/Views/Admin/Records/AddEditRecord.cshtml");
        }

        // Either adds/edits a record
        [HttpPost]
        [Authorize(Roles = "Administrator, Researcher")]
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
        [Authorize(Roles = "Administrator, Researcher")]
        public IActionResult Edit(long burialId)
        {
            var burial = _burialContext.Burials.Single(x => x.id == burialId);

            // For conditionals in view
            ViewData["Type"] = "edit";

            return View("/Views/Admin/Records/AddEditRecord.cshtml", burial); // return the AddRecord view along with the record for the single entry
        }

        // Deletes a record
        [HttpPost]
        [Authorize(Roles = "Administrator, Researcher")]
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