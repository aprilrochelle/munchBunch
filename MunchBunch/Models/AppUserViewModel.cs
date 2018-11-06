using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MunchBunch.Models
{
    public class AppUserViewModel
    {
        public AppUser AppUser { get; set; }

        public IFormFile Image { get; set; }

        public AppUserViewModel() {

        }
    }
}