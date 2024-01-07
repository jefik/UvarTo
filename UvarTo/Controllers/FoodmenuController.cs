using Microsoft.AspNetCore.Mvc;

namespace UvarTo.Web.Controllers
{
    public class FoodmenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
