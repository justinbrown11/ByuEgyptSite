using ByuEgyptSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ByuEgyptSite.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly ILogger<AnalysisController> _logger;

        public AnalysisController(ILogger<AnalysisController> logger)
        {
            _logger = logger;
        }

        public IActionResult SupervisedAnalysis()
        {
            return View();
        }

        public IActionResult UnsupervisedAnalysis()
        {
            return View();
        }
    }
}
