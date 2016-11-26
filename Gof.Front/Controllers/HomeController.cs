using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gof.Persistence;
using System.Diagnostics;

namespace Gof.Front.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<string> influencers = null;
            try
            {
                IPersistence persistence = ServiceExtensions.GetPersistenceService();
                influencers = await persistence.RetrieveInfluencerListAsync();
            }
            catch(Exception ex)
            {
                //TODO:log
                influencers = new List<string>();
                Trace.TraceError("{0}: {1}", ex.Message, ex.StackTrace);
            }
            return View(influencers);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
