using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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



            AppUserViewModel appUserViewModel = new AppUserViewModel(){
              AppUser = userDetails
            };
            return View(appUserViewModel);
        }


        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit()
        {
            var currUser = await GetCurrentUserAsync();

            var usersId = currUser.Id;

            var userDetails = await _context.AppUser
                .FindAsync(usersId);


            AppUserViewModel appUserViewModel = new AppUserViewModel(){
              AppUser = userDetails,
            };

            // using (MemoryStream mStream = new MemoryStream(userDetails.Image))
            // {
            //   appUserViewModel.ImageFile = System.Net.Mime.MediaTypeNames.Image.FromStream(mStream);
            // }
            return View(appUserViewModel);
        }

        // POST: AppUsers/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUserViewModel model)
        {

            var currUser = await GetCurrentUserAsync();
            if  (ModelState.IsValid)
            {

                currUser.FirstName = model.AppUser.FirstName;
                currUser.LastName = model.AppUser.LastName;
                currUser.PrimaryLocation = model.AppUser.PrimaryLocation.ToUpper();

                if(model.ImageFile != null) {
                  using (var memoryStream = new MemoryStream())
                  {
                      await model.ImageFile.CopyToAsync(memoryStream);
                      currUser.Image = memoryStream.ToArray();
                  }
                };

                _context.Update(currUser);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details));
            }

            return RedirectToAction(nameof(Edit));

        }

    }
}