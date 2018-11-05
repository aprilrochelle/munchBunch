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

          UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel() {
            CurrentUser = currUser
          };
          return View(userFollowsViewModel);
        }

         [Authorize]
        public async Task<IActionResult> Search(string searchTerm)
        {
          // get current user
          var currUser = await GetCurrentUserAsync();
          var usersId = currUser.Id;

          string sql = $@"
            select *
            from AspNetUsers au
            left join UserFollow uf on au.Id = uf.ReceivingUserId
            where au.Id is not '{usersId}' and
            uf.RequestingUserId is not '{usersId}' and
            au.FirstName like '%{searchTerm}%' or au.LastName like '%{searchTerm}%' or au.PrimaryLocation like '%{searchTerm.ToUpper()}%'
          ";

          // get all users I can follow
          var everyoneButMeNotFriends = _context.AppUser.FromSql(sql).ToList();

          UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel()
          {
            UsersToFollow = everyoneButMeNotFriends,
            CurrentUser = currUser
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

          UserFollowsViewModel userFollowsViewModel = new UserFollowsViewModel() {
            CurrentUser = currUser,
            UsersIFollow = myBunch
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