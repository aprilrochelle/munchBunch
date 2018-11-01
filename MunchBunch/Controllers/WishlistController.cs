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
            var currUser = await GetCurrentUserAsync();
            var usersId = currUser.Id;

            var applicationDbContext = _context.Wishlist.Include(w => w.AppUser).Where(w => w.AppUserId == usersId);
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
                .Include(w => w.AppUser)
                .FirstOrDefaultAsync(w => w.WishlistId == id);
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

            //check to see if restaurant id exists in memoir table for this user
            int memoirCheck = (_context.Memoir
                .Include(m => m.AppUser)
                .Where(m => m.AppUserId == usersId).Where(m => m.RId == rid)).Count();

            //check to see if restaurant id exists in wishlist table already for this user
            int wishlistCheck = (_context.Wishlist
                .Include(w => w.AppUser)
                .Where(w => w.AppUserId == usersId).Where(w => w.RId == rid)).Count();

            if (memoirCheck == 0 && wishlistCheck == 0) {
              //if restaurant doesn't exist in wishlist or memoir table for the user, go ahead and create the new memoir
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
            } else if (wishlistCheck > 0) {
              //if the user has already saved this restaurant id in the wishlist, redirect to index
              return RedirectToAction(nameof(Index));
            } else {
              //if the restaurant id is in the user's memoirs, redirect to memoirs index
              return RedirectToAction("Index", "Memoirs", new { area = "" });
            }

        }


        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]

        public async Task<IActionResult> Delete(int wishlistid, string tried = "false")
        {
            var wishlist = await _context.Wishlist.FindAsync(wishlistid);
            _context.Wishlist.Remove(wishlist);

            await _context.SaveChangesAsync();

            if (tried == "true") {
              return RedirectToAction("CreateFromWishlist", "Memoirs", new {
               restaurantName = wishlist.RestaurantName,
               rid = wishlist.RId,
               restaurantLocation = wishlist.RestaurantLocation,
               restaurantAddress = wishlist.RestaurantAddress
                });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.WishlistId == id);
        }
    }
}
