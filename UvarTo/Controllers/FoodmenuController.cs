using Microsoft.AspNetCore.Mvc;

namespace UvarTo.Controllers
{
    public class FoodmenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
