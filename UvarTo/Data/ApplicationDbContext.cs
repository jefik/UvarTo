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
        public DbSet<UvarTo.Models.Recipes> Recept { get; set; } = default!;
    }
}