using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UvarTo.Areas.Identity.Data;
using UvarTo.Models;

namespace UvarTo.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        public IActionResult Index()
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