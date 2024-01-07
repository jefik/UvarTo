using Microsoft.EntityFrameworkCore;
using UvarTo.Infrastructure.Identity;
using UvarTo.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UvarTo.Infrastructure.Database
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