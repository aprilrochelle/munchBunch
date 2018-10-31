﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MunchBunch.Data;
using MunchBunch.Models;

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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Memoir.Include(m => m.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Memoirs/Details/5
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

            return View(memoir);
        }

        // GET: Memoirs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Memoirs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(string restaurantName, int rid, string restLocation, string restAdd)
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
                memoir.RestaurantLocation = restLocation;
                memoir.RestaurantAddress = restAdd;

                _context.Add(memoir);
                await _context.SaveChangesAsync();
                
            }
            //ViewData["AppUserId"] = new SelectList(_context.AppUser, "Id", "Id", memoir.AppUserId);
            return RedirectToAction(nameof(Edit), new {
                id = memoir.MemoirId,
                rName = restaurantName,
                r_id = rid,
                rLoc = restLocation,
                rAdd = restAdd
            });
        }

        // GET: Memoirs/Edit/5
        public async Task<IActionResult> Edit(int? id, int r_id, string rName, string rLoc, string rAdd)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memoir = await _context.Memoir.FindAsync(id);
            memoir.RestaurantName = rName;
            memoir.RestaurantLocation = rLoc;
            memoir.RestaurantAddress = rAdd;
            if (memoir == null)
            {
                return NotFound();
            }
            //ViewData["AppUserId"] = new SelectList(_context.AppUser, "Id", "Id", memoir.AppUserId);
            return View(memoir);
        }

        // POST: Memoirs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemoirId,RId,Dish,Cocktail,Comments,AppUserId")] Memoir memoir)
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
            ViewData["AppUserId"] = new SelectList(_context.AppUser, "Id", "Id", memoir.AppUserId);
            return View(memoir);
        }

        // GET: Memoirs/Delete/5
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

            return View(memoir);
        }

        // POST: Memoirs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
