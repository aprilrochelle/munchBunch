using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MunchBunch.Models
{
    public class Wishlist
    {
        [Key]
        [Display(Name = "Wishlist ID")]
        public int WishlistId { get; set; }

        [Required]
        [Display (Name = "Restaurant ID")]
        public int RId { get; set; }

        [Required]
        [Display(Name = "Restaurant")]
        public string RestaurantName { get; set; }

        [Required]
        [Display(Name = "Area")]
        public string RestaurantLocation { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string RestaurantAddress { get; set; }

        [Required]
        [Display (Name = "User")]
        public string  AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
