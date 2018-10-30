using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunchBunch.Models;
using Microsoft.Extensions.Options;

namespace MunchBunch.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<APISettings> _apiSettings;

        public HomeController(IOptions<APISettings> apiSettings){
            _apiSettings = apiSettings;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> Search(string searchQuery)
        //{
        //    List<Memoir> searchResults = new List<Memoir>();
        //    //searchResults = fetch here?
        //    return View(searchResults);
        //}
    }
}
