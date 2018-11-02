using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MunchBunch.Models
{
    public class HomeViewModel
    {
        public AppUser AppUser { get; set; }

        public HomeViewModel(AppUser appUser) {
          AppUser = appUser;
        }
    }
}