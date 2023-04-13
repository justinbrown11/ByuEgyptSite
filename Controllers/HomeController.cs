using ByuEgyptSite.Data;
using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using LinqKit;
using ByuEgyptSite.MLModel;

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
            var record = _context.Burials.Single(x => x.id == burialid);
            return View(record);
        }

        public IActionResult BurialRecordTextiles (long burialid)
        {
            var record = _context.Burials
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
                            .ThenInclude(tf => tf.TextileFunction)
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                        .ThenInclude(t => t.analysisTextiles)
                            .ThenInclude(at => at.Analysis)
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                        .ThenInclude(t => t.decorationTextiles)
                            .ThenInclude(dt => dt.Decoration)
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                        .ThenInclude(t => t.dimensionTextiles)
                            .ThenInclude(dt => dt.Dimension)
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                        .ThenInclude(t => t.photoDataTextiles)
                            .ThenInclude(tf => tf.PhotoData)
                .Include(b => b.burialTextiles)
                    .ThenInclude(bt => bt.Textile)
                        .ThenInclude(t => t.yarnManipulationTextiles)
                            .ThenInclude(yt => yt.YarnManipulation)
                .Single(x => x.id == burialid);
            return View(record);
        }

        public IActionResult BurialRecordBodyAnalysis(long burialid)
        {
            var record = _context.Burials
                .Include(b => b.bodyAnalyses)
                .Single(x => x.id == burialid);
            return View(record);
        }

        [HttpGet]
        public IActionResult BurialSummary(int? pageNumber, 
            string? textColor, string? structure, string? sex, string? depth, string? stature,
            string? ageatdeath, string? headDirection, string? burialid, string? textileFunction,
            string? hairColor, string? faceBundle)
        {
            int pageSize = 5;

            var predicate = PredicateBuilder.New<Burial>(true);

            if (!string.IsNullOrEmpty(textColor))
            {
                predicate = predicate.And(b => b.burialTextiles.Any(bt => bt.Textile.colorTextiles.Any(ct => Regex.IsMatch(ct.Color.value, textColor))));
            }

            if (!string.IsNullOrEmpty(structure))
            {
                predicate = predicate.And(b => b.burialTextiles.Any(bt => bt.Textile.structureTextiles.Any(st => Regex.IsMatch(st.Structure.value, structure))));
            }

            if (!string.IsNullOrEmpty(stature))
            {
                predicate = predicate.And(b => b.bodyAnalyses.Any(ba => Regex.IsMatch(ba.estimatestature, stature)));
            }

            if (!string.IsNullOrEmpty(sex))
            {
                predicate = predicate.And(b => Regex.IsMatch(b.sex, sex));
            }

            if (!string.IsNullOrEmpty(depth))
            {
                predicate = predicate.And(b => Regex.IsMatch(b.depth, depth));
            }

            if (!string.IsNullOrEmpty(ageatdeath))
            {
                predicate = predicate.And(b => Regex.IsMatch(b.ageatdeath, ageatdeath));
            }

            if (!string.IsNullOrEmpty(headDirection))
            {
                predicate = predicate.And(b => Regex.IsMatch(b.headdirection, headDirection));
            }

            if (!string.IsNullOrEmpty(burialid))
            {
                int burialIdInt;
                if (int.TryParse(burialid, out burialIdInt))
                {
                    predicate = predicate.And(b => b.burialid == burialIdInt);
                }
            }

            if (!string.IsNullOrEmpty(textileFunction))
            {
                predicate = predicate.And(b => b.burialTextiles.Any(bt => bt.Textile.textileFunctionTextiles.Any(tf => Regex.IsMatch(tf.TextileFunction.value, textileFunction))));
            }

            if (!string.IsNullOrEmpty(hairColor))
            {
                predicate = predicate.And(b => Regex.IsMatch(b.haircolor, hairColor));
            }

            if (!string.IsNullOrEmpty(faceBundle))
            {
                predicate = predicate.And(b => Regex.IsMatch(b.facebundles, faceBundle));
            }

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
                            .ThenInclude(tf => tf.TextileFunction)
                .Where(predicate)
                .ToList();


            var paginatedBurials = PaginatedList<Burial>.Create(burials,
                pageNumber ?? 1, pageSize);

            ViewData["textColor"] = textColor;
            ViewData["structure"] = structure;
            ViewData["sex"] = sex;
            ViewData["depth"] = depth;
            ViewData["stature"] = stature;
            ViewData["ageAtDeath"] = ageatdeath;
            ViewData["headDirection"] = headDirection;
            ViewData["burialId"] = burialid;
            ViewData["textileFunction"] = textileFunction;
            ViewData["hairColor"] = hairColor;
            ViewData["faceBundle"] = faceBundle;

            return View(paginatedBurials);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}