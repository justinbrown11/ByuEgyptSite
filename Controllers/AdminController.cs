using Microsoft.AspNetCore.Mvc;

namespace ByuEgyptSite.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Records()
        {
            return View("Home/BurialSummary");
        }

        public IActionResult AddRecord()
        {
            return View("Admin/Records/AddRecord");
        }
    }
}
