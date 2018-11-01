using System;
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

namespace MunchBunch.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public WishlistsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Wishlists
        [Authorize]
        public async Task<IActionResult> Index()
        {
          //filter by current user
            var applicationDbContext = _context.Wishlist.Include(m => m.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Wishlists/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist
                .Include(m => m.AppUser)
                .FirstOrDefaultAsync(m => m.WishlistId == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // GET: Wishlists/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string restaurantName, int rid, string restaurantLocation, string restaurantAddress)
        {
            //get current user
            var currUser = await GetCurrentUserAsync();
            var usersId = currUser.Id;

            Wishlist myWishlist = new Wishlist();

            if (ModelState.IsValid)
            {
                myWishlist.RId = rid;
                myWishlist.AppUserId = usersId;
                myWishlist.RestaurantName = restaurantName;
                myWishlist.RestaurantLocation = restaurantLocation;
                myWishlist.RestaurantAddress = restaurantAddress;

                _context.Add(myWishlist);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // // GET: Memoirs/Edit/5
        // [Authorize]
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var memoir = await _context.Memoir.FindAsync(id);

        //     if (memoir == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(memoir);
        // }

        // // POST: Memoirs/Edit/5
        // // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [Authorize]
        // public async Task<IActionResult> Edit(int id, Memoir memoir)
        // {
        //     if (id != memoir.MemoirId)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(memoir);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!MemoirExists(memoir.MemoirId))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(memoir);
        // }

        // GET: Wishlists/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist
                .Include(m => m.AppUser)
                .FirstOrDefaultAsync(m => m.WishlistId == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishlist = await _context.Wishlist.FindAsync(id);
            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.WishlistId == id);
        }
    }
}
