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
using MunchBunch.Models.UserFollowsViewModels;

namespace MunchBunch.Controllers
{
    public class UserFollowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public UserFollowsController(ApplicationDbContext context, UserManager<AppUser> userManager){
            _context = context;
            _userManager = userManager;
        }

        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Authorize]
         public async Task<IActionResult> Index()
        {
            // get current user
            var currUser = await GetCurrentUserAsync();

            UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel(currUser);
            return View(userFollowsViewModel);
        }
    }
}