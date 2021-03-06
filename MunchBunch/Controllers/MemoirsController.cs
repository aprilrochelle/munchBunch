﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MunchBunch.Data;
using MunchBunch.Models;
using MunchBunch.Models.MemoirViewModels;

namespace MunchBunch.Controllers
{
    public class MemoirsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public MemoirsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Memoirs
        [Authorize]
        public async Task<IActionResult> Index()
        {
          //filter by current user
            var currUser = await GetCurrentUserAsync();
            var usersId = currUser.Id;

            var applicationDbContext = _context.Memoir.Include(m => m.AppUser).Where(m => m.AppUserId == usersId);
            var memoirList = await applicationDbContext.ToListAsync();

            MemoirListViewModel memoirListViewModel = new MemoirListViewModel() {
              Memoirs = memoirList
            };

            return View(memoirListViewModel);
        }

        // GET: Memoirs/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memoir = await _context.Memoir
                .Include(m => m.AppUser)
                .FirstOrDefaultAsync(m => m.MemoirId == id);
            if (memoir == null)
            {
                return NotFound();
            }

            MemoirEditDetailDeleteViewModel memoirEditDetailDeleteViewModel = new MemoirEditDetailDeleteViewModel(memoir);
            return View(memoirEditDetailDeleteViewModel);
        }

        // GET: Memoirs/Create
        [Authorize]
        public IActionResult Create()
        {
            MemoirCreateViewModel memoirCreateViewModel = new MemoirCreateViewModel();
            return View();
        }

        // POST: Memoirs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string restaurantName, int rid, string restaurantLocation, string restaurantAddress)
        {
            //get current user
            var currUser = await GetCurrentUserAsync();
            var usersId = currUser.Id;

            //check to see if restaurant id exists in memoir table for this user
            int memoirCheck = (_context.Memoir
                .Include(m => m.AppUser)
                .Where(m => m.AppUserId == usersId).Where(m => m.RId == rid)).Count();

            //check to see if restaurant id exists in wishlist table already for this user
            int wishlistCheck = (_context.Wishlist
                .Include(w => w.AppUser)
                .Where(w => w.AppUserId == usersId).Where(w => w.RId == rid)).Count();

            //if restaurant doesn't exist in wishlist or memoir table for the user, go ahead and create the new memoir
            if (memoirCheck == 0 && wishlistCheck == 0) {

              Memoir memoir = new Memoir();

              if (ModelState.IsValid)
              {
                  memoir.RId = rid;
                  memoir.Comments = "";
                  memoir.AppUserId = usersId;
                  memoir.RestaurantName = restaurantName;
                  memoir.RestaurantLocation = restaurantLocation;
                  memoir.RestaurantAddress = restaurantAddress;

                  _context.Add(memoir);
                  await _context.SaveChangesAsync();
              }

              return RedirectToAction(nameof(Edit), new {
                  id = memoir.MemoirId
              });
            } else if (memoirCheck > 0) {
              //if the user has already saved a memoir for this restaurant id, redirect to index
              return RedirectToAction(nameof(Index));
            } else {
              //if the restaurant id is in the user's wishlist, redirect to wishlist index
              return RedirectToAction("Index", "Wishlists", new { area = "" });
            }

        }

        // GET: Memoirs/Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateFromWishlist(string restaurantName, int rid, string restaurantLocation, string restaurantAddress)
        {
            //get current user
            var currUser = await GetCurrentUserAsync();
            var usersId = currUser.Id;

            Memoir memoir = new Memoir();

            if (ModelState.IsValid)
            {
                memoir.RId = rid;
                memoir.Comments = "";
                memoir.AppUserId = usersId;
                memoir.RestaurantName = restaurantName;
                memoir.RestaurantLocation = restaurantLocation;
                memoir.RestaurantAddress = restaurantAddress;

                _context.Add(memoir);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Edit), new {
                id = memoir.MemoirId
            });
        }

        // GET: Memoirs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memoir = await _context.Memoir.FindAsync(id);

            if (memoir == null)
            {
                return NotFound();
            }
            MemoirEditDetailDeleteViewModel memoirEditDetailDeleteViewModel = new MemoirEditDetailDeleteViewModel(memoir);
            return View(memoirEditDetailDeleteViewModel);
        }

        // POST: Memoirs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Memoir memoir)
        {
            if (id != memoir.MemoirId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memoir);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemoirExists(memoir.MemoirId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            MemoirEditDetailDeleteViewModel memoirEditDetailDeleteViewModel = new MemoirEditDetailDeleteViewModel(memoir);
            return View(memoirEditDetailDeleteViewModel);
        }

        // GET: Memoirs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memoir = await _context.Memoir
                .Include(m => m.AppUser)
                .FirstOrDefaultAsync(m => m.MemoirId == id);
            if (memoir == null)
            {
                return NotFound();
            }
            MemoirEditDetailDeleteViewModel memoirEditDetailDeleteViewModel = new MemoirEditDetailDeleteViewModel(memoir);
            return View(memoirEditDetailDeleteViewModel);
        }

        // POST: Memoirs/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memoir = await _context.Memoir.FindAsync(id);
            _context.Memoir.Remove(memoir);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemoirExists(int id)
        {
            return _context.Memoir.Any(e => e.MemoirId == id);
        }
    }
}
