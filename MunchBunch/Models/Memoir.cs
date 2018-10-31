using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MunchBunch.Models
{
    public class Memoir
    {
        [Key]
        [Display(Name = "Memoir ID")]
        public int MemoirId { get; set; }

        [Required]
        [Display (Name = "Restaurant ID")]
        public int RId { get; set; }

        public string Dish { get; set; }

        public string Cocktail { get; set; }

        [Required]
        public string Comments { get; set; }

        [Display(Name = "Restaurant")]
        public string RestaurantName { get; set; }

        [Display(Name = "Area")]
        public string RestaurantLocation { get; set; }

        [Display(Name = "Address")]
        public string RestaurantAddress { get; set; }

        [Required]
        [Display (Name = "User")]
        public string  AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
