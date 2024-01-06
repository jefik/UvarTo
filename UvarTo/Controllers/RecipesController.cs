using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UvarTo.Data;
using UvarTo.Data.Migrations;
using UvarTo.Models;

namespace UvarTo.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment hostingenvironment;

        public RecipesController(ApplicationDbContext context, IWebHostEnvironment hc) 
        {
            _context = context;
            hostingenvironment = hc;
        }



        //else
        //    var userItems = _context.Recept.Where(item => item.Id == userId).ToList();

        //return View(userItems);
        private string GetCurrentId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }
        public async Task<IActionResult> UserItems()
        {

            var userIdString = GetCurrentId();
            //var userId = Convert.ToInt32(userIdString);
            //var userIdGuid = new Guid(userIdString); // Convert the string to a Guid
            //var userId = userIdGuid.GetHashCode(); // Convert Guid to int
            var userItems = _context.Recept.Where(item => item.userId == userIdString).ToList();

            ViewBag.UserIdString = userIdString;
            //ViewBag.UserIdGuid = userIdGuid;
            //ViewBag.UserId = userId;
            return View("userItems", userItems);


        }


        // GET: Recipes
        public async Task<IActionResult> Index()
        {
              return _context.Recept != null ? 
                          View(await _context.Recept.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Recept'  is null.");
        }
        // GET: Recepts/ShowSearchForm
        public IActionResult Search()
        {
            return View("Search");
        }
        // POST: Recepts/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var searchResults = await _context.Recept
            .Where(j => j.RecipeText.Contains(SearchPhrase))
            .ToListAsync();

            return View("Search", searchResults);
        }
        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recept == null)
            {
                return NotFound();
            }

            var recept = await _context.Recept
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recept == null)
            {
                return NotFound();
            }

            return View(recept);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(recipeviewmodel recipe1)
        {
            var userId = GetCurrentId();
            if (recipe1.photo != null)
                {
                    string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + recipe1.photo.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await recipe1.photo.CopyToAsync(stream);
                    }

                    Recipes r = new Recipes
                    {
                        // Set other properties here
                        Id = recipe1.Id,
                        userId = userId,
                        Difficulty = recipe1.Difficulty,
                        CookTime = recipe1.CookTime,
                        RecipeName = recipe1.RecipeName,
                        RecipeCategory = recipe1.RecipeCategory,
                        RecipeText = recipe1.RecipeText,
                        ImageUrl = @"\images\" + uniqueFileName
                    };

                    _context.Recept.Add(r);
                    await _context.SaveChangesAsync();
                    ViewBag.Success = "Recipe Added";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Photo", "Please select a file.");
                }
            

            return View(recipe1);
        }

        // GET: Recepts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recept == null)
            {
                return NotFound();
            }

            var existingRecept = await _context.Recept.FindAsync(id);
            if (existingRecept == null)
            {
                return NotFound();
            }

            var viewModel = new recipeviewmodel
            {
                // Populate the properties of the recipeviewmodel
                Id = existingRecept.Id,
                // Assign other properties from existingRecept to recipeviewmodel
                
                Difficulty = existingRecept.Difficulty,
                CookTime = existingRecept.CookTime,
                RecipeName = existingRecept.RecipeName,
                RecipeCategory = existingRecept.RecipeCategory,
                RecipeText = existingRecept.RecipeText,
                

                // Initialize PhotoModel with null or any default values as needed
                photo = null
            };

            return View(viewModel);
        }

        // POST: Recepts/Edit/5

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
                var existingRecipe = await _context.Recept.FindAsync(recipes.Id);
                if (existingRecipe == null)
                {
                    return NotFound();
                }

                // If the user uploaded a new image
                if (recipes.photo != null)
                {
                    string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + recipes.photo.FileName;
                    string filePath = Path.Combine(hostingenvironment.WebRootPath, "images", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await recipes.photo.CopyToAsync(stream);
                    }

                    string imageUrl = @"\images\" + uniqueFileName;
                    // Update other properties
                    existingRecipe.Difficulty = recipes.Difficulty;
                    existingRecipe.CookTime = recipes.CookTime;
                    existingRecipe.RecipeName = recipes.RecipeName;
                    existingRecipe.RecipeCategory = recipes.RecipeCategory;
                    existingRecipe.ImageUrl = imageUrl;
                    existingRecipe.RecipeText = recipes.RecipeText;
                }

                _context.Update(existingRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(recipes);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recept == null)
            {
                return NotFound();
            }

            var recept = await _context.Recept
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recept == null)
            {
                return NotFound();
            }

            return View(recept);
        }

        // POST: Recepts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recept == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Recept'  is null.");
            }
            var recept = await _context.Recept.FindAsync(id);
            if (recept != null)
            {
                _context.Recept.Remove(recept);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceptExists(int id)
        {
          return (_context.Recept?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
