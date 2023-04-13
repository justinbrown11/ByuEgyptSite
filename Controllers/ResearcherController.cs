using ByuEgyptSite.Controllers;
using ByuEgyptSite.Data;
using ByuEgyptSite.MLModel;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Text.Json;


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
        [HttpGet]
        public IActionResult SupervisedAnalysis()
        {
            return View("/Views/Researcher/SupervisedAnalysis.cshtml", new UserData());
        }


        [HttpPost]
        public IActionResult SupervisedAnalysis(UserData data)
        {
            return RedirectToAction("AlterData","Researcher", data);
        }

        public IActionResult AlterData(UserData data)
        {
            var temp = data;
            var json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return Content(json, "application/json");
        }


        //[HttpGet]
        //public IActionResult AlterData(UserData data)
        //{
        //    var temp = data;
        //    var json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        //    return RedirectToAction("AlterData", new {data = json});
        //}

        //[HttpPost]
        //public IActionResult AlterData(UserData data)
        //{
        //    var temp = data;
        //    var json = JsonSerializer.Serialize(temp, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        //    return RedirectToAction("Score", "Inference", new { data = json });
        //}


        //[HttpGet]
        //public IActionResult Test(UserData data)
        //{
        //    var temp = data;

        //    return View(temp);
        //}

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
        // and return a view
        public IActionResult AddRecord(
         string? squarenorthsouth,
         string? headdirection,
         string? sex,
         string? northsouth,
         string? depth,
         string? eastwest,
         string? adultsubadult,
         string? facebundles,
         string? southtohead,
         string? preservation,
         string? fieldbookpage,
         string? squareeastwest,
         string? goods,
         string? text,
         string? wrapping,
         string? haircolor,
         string? westtohead,
         string? samplescollected,
         string? area,
         long? burialid,
         string? length,
         string? burialnumber,
         string? dataexpertinitials,
         string? westtofeet,
         string? ageatdeath,
         string? southtofeet,
         string? excavationrecorder,
         string? photos,
         string? hair,
         string? burialmaterials,
         DateTime? dateofexcavation,
         string? fieldbookexcavationyear,
         string? clusternumber,
         string? shaftnumber,
         string? burialmainid
)
        {
            //if (ModelState.IsValid) // If entry is valid, add the object and return confirmation view
            //{
            //    byte[] bytes = Guid.NewGuid().ToByteArray();
            //    long randomId = BitConverter.ToInt64(bytes, 0);

            //    _burialContext.Add(bur);
            //    _burialContext.SaveChanges();

                return RedirectToAction("BurialSummary", "Home");
            //}
            //else // If entry is invalid, return AddRecord view with record to be added
            //{
            //    return View(bur);
            //}
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