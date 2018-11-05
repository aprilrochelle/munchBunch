using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MunchBunch.Models.UserFollowsViewModels
{
    public class UserFollowsViewModel
    {

        public List<UserFollow> UsersIFollow { get; set; }

        public List<UserFollow> UsersFollowingMe { get; set; }

        public List<AppUser> UsersToFollow { get; set; }

        public AppUser CurrentUser { get; set; }

        public UserFollowsViewModel() {

        }
    }
}