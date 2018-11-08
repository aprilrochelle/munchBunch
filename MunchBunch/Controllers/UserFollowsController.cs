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
            select *
            from AspNetUsers au
            left join UserFollow uf on au.Id = uf.ReceivingUserId
            where au.Id is not '{usersId}' and
            uf.RequestingUserId is not '{usersId}'
            group by au.Id";

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