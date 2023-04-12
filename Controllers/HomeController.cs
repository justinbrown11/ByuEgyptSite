using ByuEgyptSite.Data;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

        public IActionResult BurialRecord(long burialid)
        {
            var record = _context.Burials.Include(b => b.burialTextiles).Single(x => x.id == burialid);
            return View(record);
        }

        [HttpGet]
        public IActionResult BurialSummary(int? pageNumber, 
            string? textColor, string? structure, string? sex, string? depth, string? stature,
            string? ageatdeath, string? headDirection, string? burialid, string? textileFunction,
            string? hairColor, string? faceBundle)
        {
            int pageSize = 5;

            var burials = _context.Burials
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                    .ThenInclude(t => t.colorTextiles)
                    .ThenInclude(ct => ct.Color)
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                    .ThenInclude(t => t.structureTextiles)
                    .ThenInclude(st => st.Structure)
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                    .ThenInclude(t => t.textileFunctionTextiles)
                    .ThenInclude(tf => tf.Textile)
                .Where(b =>
                    (b.burialTextiles.Any(bt => bt.Textile.colorTextiles.Any(ct => Regex.IsMatch(ct.Color.value, textColor ?? ".*")))) ||
                    (b.burialTextiles.Any(bt => bt.Textile.structureTextiles.Any(st => Regex.IsMatch(st.Structure.value, structure ?? ".*")))) ||
                    (Regex.IsMatch(b.sex, sex ?? ".*")) ||
                    (Regex.IsMatch(b.depth, depth ?? ".*")) ||
                    (Regex.IsMatch(b.ageatdeath, ageatdeath ?? ".*")) ||
                    (Regex.IsMatch(b.headdirection, headDirection ?? ".*")) ||
                    (Regex.IsMatch(b.burialid.ToString(), burialid ?? ".*")) ||
                    (b.burialTextiles.Any(bt => bt.Textile.textileFunctionTextiles.Any(tf => Regex.IsMatch(tf.TextileFunction.value, textileFunction ?? ".*")))) ||
                    (Regex.IsMatch(b.haircolor, hairColor ?? ".*")) ||
                    (Regex.IsMatch(b.facebundles, faceBundle ?? ".*"))
                ).ToList();


            var paginatedBurials = PaginatedList<Burial>.Create(burials,
                pageNumber ?? 1, pageSize);

            return View(paginatedBurials);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}