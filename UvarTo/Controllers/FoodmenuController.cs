using Microsoft.AspNetCore.Mvc;
using UvarTo.Domain.Entities;
using UvarTo.Application.Implementation;
using UvarTo.Application.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace UvarTo.Web.Controllers
{
    public class FoodmenuController : Controller
    {
        private readonly IFoodmenuService _foodmenuService;

        public FoodmenuController(IFoodmenuService foodmenuService)
        {
            _foodmenuService = foodmenuService;
        }
        public async Task<IActionResult> UserItems()
        {
            var userId = _foodmenuService.GetCurrentId(); // Implement the logic to get the current user's ID here

            var userItems = _foodmenuService.UserItems(userId);

            ViewBag.UserIdString = userId;
            return View("userItems", userItems);
        }

        public async Task<IActionResult> Index()
        {
            var tips = await _foodmenuService.GetAllFoodmenus();

            if (tips != null)
            {
                return View(tips);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Tips' is null.");
            }

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodmenus = await _foodmenuService.GetFoodmenuById(id.Value);

            if (foodmenus == null)
            {
                return NotFound();
            }

            return View(foodmenus);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Foodmenu foodmenus)
        {

            await _foodmenuService.AddFoodmenu(foodmenus);
            ViewBag.Success = "Foodmenu added";
            return RedirectToAction(nameof(Index));


            return View(foodmenus);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existingFoodmenu = await _foodmenuService.GetFoodmenuById(id.Value);
            if (existingFoodmenu == null)
            {
                return NotFound();
            }

            var foodmenus = new Foodmenu
            {
                id = existingFoodmenu.id,
                FoodmenuName = existingFoodmenu.FoodmenuName,
                FoodmenuText = existingFoodmenu.FoodmenuText
            };

            return View(foodmenus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Foodmenu foodmenus)
        {
            if (id != foodmenus.id)
            {
                return NotFound();
            }


            var result = await _foodmenuService.UpdateFoodmenu(foodmenus);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }


            return View(foodmenus);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFoodmenu(int id)
        {
            var userId = _foodmenuService.GetCurrentId();

            var isDeleted = await _foodmenuService.DeleteFood(userId, id);

            if (!isDeleted)
            {
                return NotFound(); 
            }

            TempData["SuccessMessage"] = "Foodmenu deleted successfully.";
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var foodmenus = await _foodmenuService.GetFoodmenuById(id.Value);
        //    if (foodmenus == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(foodmenus);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var result = await _foodmenuService.DeleteFoodmenu(id);
        //    if (result)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Foodmenu' is null.");
        //    }
        //}
    }
}
