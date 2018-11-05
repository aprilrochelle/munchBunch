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
            left join UserFollow uf on au.Id = uf.UserId
            where au.Id is not '{currUser.Id}' and
            uf.FollowerId is not '{currUser.Id}' and
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
    }
}