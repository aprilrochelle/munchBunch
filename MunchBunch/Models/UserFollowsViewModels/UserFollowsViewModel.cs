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

        public List<UserFollow> Followships { get; set; }

        public List<AppUser> UsersToFollow { get; set; }

        public AppUser CurrentUser { get; set; }

        public UserFollowsViewModel() {

        }
    }
}