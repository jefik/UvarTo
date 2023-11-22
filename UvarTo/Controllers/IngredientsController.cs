using Microsoft.AspNetCore.Mvc;

namespace UvarTo.Controllers
{
    public class IngredientsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
