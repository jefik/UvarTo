using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UvarTo.Areas.Identity.Data;
using UvarTo.Models;

namespace UvarTo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Recipes> Recept { get; set; }
        public DbSet<Foodmenu> Foodmenu { get; set; }
        public DbSet<Tips> Tips { get; set; }
        
    }
}