using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UvarTo.Application.Abstraction;
using UvarTo.Application.Implementation;
using UvarTo.Domain.Entities;
using UvarTo.Infrastructure.Database;
using UvarTo.Infrastructure.Identity;
using UvarTo.Web.Models;

namespace UvarTo.Controllers
{
    public class YourCommonViewModel
    {
        public List<Recipes> Recipes { get; set; }
        public List<Tips> Tips { get; set; }

        public YourCommonViewModel()
        {
            Recipes = new List<Recipes>();
            Tips = new List<Tips>();
        }
    }
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IRecipesService _recipeService;
        private readonly ITipsService _tipsService;
        private readonly IFoodmenuService _foodmenuService;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IRecipesService recipeService, ITipsService tipsService, IFoodmenuService foodmenuService)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
            _recipeService = recipeService;
            _tipsService = tipsService;
            _foodmenuService = foodmenuService;

        }

        //public async Task<IActionResult> Index()
        //{
        //    ApplicationUser user = _userManager.GetUserAsync(User).Result;

        //    // Check if the user is not null to avoid null reference exceptions
        //    if (user != null)
        //    {
        //        ViewData["UserFirstName"] = user.FirstName;
        //    }


        //    return View();

        //}
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                ViewData["UserFirstName"] = user.FirstName;
            }
            var viewModel = new Together();

            viewModel.Recipes = await _recipeService.GetAllRecipes();
            viewModel.Tips = await _tipsService.GetAllTips();
            viewModel.Foodmenus = await _foodmenuService.GetAllFoodmenus();

            if (viewModel.Recipes == null || viewModel.Tips == null)
            {
                return Problem("Entity set is null.");
            }

            return View(viewModel);
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
