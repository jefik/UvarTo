using Microsoft.AspNetCore.Mvc;
using UvarTo.Domain.Entities;
using UvarTo.Application.Implementation;
using UvarTo.Application.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace UvarTo.Web.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipesService _recipeService;
        private readonly ISearchRService _searchRService;

        public RecipesController(IRecipesService recipeService, ISearchRService searchRService)
        {
            _recipeService = recipeService;
            _searchRService = searchRService;
        }

        public async Task<IActionResult> UserItems()
        {
            var userId = _recipeService.GetCurrentId(); // Implement the logic to get the current user's ID here

            var userItems = _recipeService.GetUserRecipes(userId);

            ViewBag.UserIdString = userId;
            return View("userItems", userItems);
        }
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            var searchResults = await _searchRService.GetRSearchResults(searchPhrase);
            return View("Search", searchResults);
        }

        public async Task<IActionResult> Index()
        {
            var recipes = await _recipeService.GetAllRecipes();

            if (recipes != null)
            {
                return View(recipes);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Recept' is null.");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recept = await _recipeService.GetRecipeById(id.Value);

            if (recept == null)
            {
                return NotFound();
            }

            return View(recept);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(recipeviewmodel recipe1)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _recipeService.AddRecipe(recipe1);
                    ViewBag.Success = "Recipe added";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("Photo", ex.Message);
                }
            }

            return View(recipe1);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var viewModel = await _recipeService.GetRecipeForEdit(id.Value);
                return View(viewModel);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, recipeviewmodel recipes)
        {
            if (id != recipes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _recipeService.UpdateRecipe(recipes);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return View(recipes);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecipes(int id)
        {
            var userId = _recipeService.GetCurrentId();

            var isDeleted = await _recipeService.DeleteRecipe(userId, id);

            if (!isDeleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Recipe deleted successfully.";
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var recept = await _recipeService.GetRecipeById(id.Value);
        //    if (recept == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(recept);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var deleted = await _recipeService.DeleteRecipe(id);
        //    if (!deleted)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Recept' is null.");
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        // idk why is this function here, but here we go YAAAHOOO

        //private bool ReceptExists(int id)
        //{
        //    return (_context.Recept?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
