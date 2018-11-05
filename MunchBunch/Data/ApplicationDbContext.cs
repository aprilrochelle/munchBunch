using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MunchBunch.Models;

namespace MunchBunch.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Memoir> Memoir { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<UserFollow> UserFollow { get; set; }

    }
}
