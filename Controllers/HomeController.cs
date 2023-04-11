using ByuEgyptSite.Data;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ByuEgyptSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context { get; set; }
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext temp)
        {
            _logger = logger;
            _context = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BurialSummary(int? pageNumber)
        {
            int pageSize = 5;

            var burials = PaginatedList<Burial>.Create(_context.Burials.ToList(),
                pageNumber ?? 1, pageSize);

            return View(burials);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}