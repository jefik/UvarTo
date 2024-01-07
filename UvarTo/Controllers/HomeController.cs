using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UvarTo.Infrastructure.Database;
using UvarTo.Infrastructure.Identity;
using UvarTo.Web.Models;

namespace UvarTo.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            // Check if the user is not null to avoid null reference exceptions
            if (user != null)
            {
                ViewData["UserFirstName"] = user.FirstName;
            }


            return View();

        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Foodmenu()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
