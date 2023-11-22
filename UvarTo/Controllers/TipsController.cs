using Microsoft.AspNetCore.Mvc;

namespace UvarTo.Controllers
{
    public class TipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
