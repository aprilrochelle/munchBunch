using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MunchBunch.Data;
using MunchBunch.Models;

namespace MunchBunch.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public AppUsersController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: AppUsers/Details/5
        public async Task<IActionResult> Details()
        {

            var currUser = await GetCurrentUserAsync();

            var usersId = currUser.Id;

            var userDetails = await _context.AppUser
                .FindAsync(usersId);

            return View(userDetails);
        }


        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit()
        {
            var currUser = await GetCurrentUserAsync();

            var usersId = currUser.Id;

            var userDetails = await _context.AppUser
                .FindAsync(usersId);

            return View(userDetails);
        }

        // POST: AppUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("FirstName, LastName, PrimaryLocation")] AppUser appUser)
        {
            var currUser = await GetCurrentUserAsync();

            currUser.FirstName = appUser.FirstName;
            currUser.LastName = appUser.LastName;
            currUser.PrimaryLocation = appUser.PrimaryLocation;

            if (ModelState.IsValid)
            {

                _context.Update(currUser);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details));
            }

            return RedirectToAction(nameof(Edit));

        }

    }
}