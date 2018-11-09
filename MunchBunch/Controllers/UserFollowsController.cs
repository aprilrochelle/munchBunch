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
using Microsoft.EntityFrameworkCore;

namespace MunchBunch.Controllers
{
  public class UserFollowsController : Controller
  {
    private readonly ApplicationDbContext _context;

    private readonly UserManager<AppUser> _userManager;

    public UserFollowsController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    private Task<AppUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

    [Authorize]
    public async Task<IActionResult> Index()
    {

      // get current user
      var currUser = await GetCurrentUserAsync();

      UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel()
      {
        User = currUser
      };
      return View(userFollowsViewModel);
    }

    [Authorize]
    public async Task<IActionResult> Search(string searchTerm)
    {
      // get current user
      var currUser = await GetCurrentUserAsync();
      var usersId = currUser.Id;

      string sql1 = $@"
            select Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, 
                LockoutEnabled, AccessFailedCount, Discriminator, FirstName, LastName, PrimaryLocation, [Image]
            from AspNetUsers au
            left join UserFollow uf on au.Id = uf.ReceivingUserId
            where not au.Id = '{usersId}' or
            not uf.RequestingUserId = '{usersId}'
            group by au.Id, au.FirstName, au.LastName, au.PrimaryLocation, au.UserName, au.NormalizedUserName, 
            au.Email, au.NormalizedEmail, au.EmailConfirmed, au.PasswordHash,
                au.SecurityStamp, au.ConcurrencyStamp, au.PhoneNumber, au.PhoneNumberConfirmed, 
                au.TwoFactorEnabled, au.LockoutEnd, 
                au.LockoutEnabled, au.AccessFailedCount, au.Discriminator, au.[Image]";

      var everyoneNotMe = _context.AppUser.FromSql(sql1).ToList();

      List<AppUser> usersToFollow = new List<AppUser>();

      foreach (var person in everyoneNotMe) {
        if (person.FirstName.Contains(searchTerm) || person.LastName.Contains(searchTerm) || person.PrimaryLocation.Contains(searchTerm.ToUpper())) {
          usersToFollow.Add(person);
        }
      }

      UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel()
      {
        UsersToFollow = usersToFollow,
        User = currUser
      };
      return View(userFollowsViewModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Follow(string userid)
    {
      // get current user
      var currUser = await GetCurrentUserAsync();
      var currUsersId = currUser.Id;

        // double check to make sure I'm not following them already
        var doIFollow = (_context.UserFollow
            .Where(f => f.ReceivingUserId == userid)
            .Where(f => f.RequestingUserId == currUsersId)).Count();

        if(doIFollow == 0)
            {
                UserFollow uFollow = new UserFollow();

                if (ModelState.IsValid)
                {
                    uFollow.ReceivingUserId = userid;
                    uFollow.RequestingUserId = currUsersId;

                    _context.Add(uFollow);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(ShowMyBunch));
            }

            return RedirectToAction(nameof(ShowMyBunch));
        }

    [Authorize]
    public async Task<IActionResult> ShowMyBunch()
    {
      // get current user
      var currUser = await GetCurrentUserAsync();
      var usersId = currUser.Id;

      var myBunch = _context.UserFollow
      .Include(u => u.ReceivingUser)
      .Include(u => u.RequestingUser)
      .Where(u => u.RequestingUserId == usersId).ToList();

      UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel()
      {
        User = currUser,
        UsersIFollow = myBunch
      };

      return View(userFollowsViewModel);
    }


    [Authorize]
    public async Task<IActionResult> SeeMemoirs(string userid, AppUser user)
    {
      var muncherMemoirs = _context.Memoir.Include(m => m.AppUser).Where(m => m.AppUserId == userid);
      var memoirList = await muncherMemoirs.ToListAsync();

      UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel()
      {
        MuncherMemoirs = memoirList,
        UserId = userid,
        User = user
      };

      return View(userFollowsViewModel);
    }

    [Authorize]
    public async Task<IActionResult> MemoirDetails(int id, string userid, AppUser user)
    {

      var memoir = await _context.Memoir
          .Include(m => m.AppUser)
          .FirstOrDefaultAsync(m => m.MemoirId == id);

      UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel()
      {
        FeaturedMemoir = memoir,
        UserId = userid,
        User = user
      };

      return View(userFollowsViewModel);
    }



    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Unfollow(string userid)
    {
      // get current user
      var currUser = await GetCurrentUserAsync();
      var currUsersId = currUser.Id;

      var userToUnfollow = await _context.UserFollow
      .Where(u => u.RequestingUserId == currUsersId && u.ReceivingUserId == userid).FirstOrDefaultAsync();

      _context.UserFollow.Remove(userToUnfollow);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(ShowMyBunch));

    }
  }
}