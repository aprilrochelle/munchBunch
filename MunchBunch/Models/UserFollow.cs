using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MunchBunch.Models
{
    public class UserFollow
    {
        [Key]
        [Display(Name = "Following ID")]
        public int UserFollowId { get; set; }

        [Required]
        [Display (Name = "User ID")]
        public string ReceivingUserId { get; set; }
        public AppUser ReceivingUser { get; set; }

        [Required]
        [Display(Name = "Follower Id")]
        public string RequestingUserId { get; set; }
        public AppUser RequestingUser { get; set; }
    }
}
