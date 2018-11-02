using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MunchBunch.Models;
using Microsoft.Extensions.Options;
using MunchBunch.Data;
using Microsoft.AspNetCore.Identity;

namespace MunchBunch.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<APISettings> _apiSettings;
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public HomeController(IOptions<APISettings> apiSettings, ApplicationDbContext context, UserManager<AppUser> userManager){
            _apiSettings = apiSettings;
            _context = context;
            _userManager = userManager;
        }

        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Authorize]
         public async Task<IActionResult> Index()
        {
            // get current user
            var currUser = await GetCurrentUserAsync();

            HomeViewModel homeViewModel = new HomeViewModel(currUser);
            return View(homeViewModel);
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

    }
}
