using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
            .Where(j => j.RecipeName.Contains(SearchPhrase))
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

        //private string UploadFile(recipeviewmodel recipe1)
        //{
        //    String filename = "";
        //    if (recipe1.photo != null)
        //    {
        //        String uploadfolder = Path.Combine(hostingenvironment.WebRootPath, "images");
        //        filename = Guid.NewGuid().ToString() + "_" + recipe1.photo.FileName;
        //        String filepath = Path.Combine(uploadfolder, filename);
        //        recipe1.photo.CopyTo(new FileStream(filepath, FileMode.Create));
        //        //using (var fileStream = new FileStream(filepath, FileMode.Create))
        //        //{
        //        //    recipe1.photo.CopyTo(fileStream);
        //        //}
        //    }
        //    return filename;
        //}
        // NEFUNKČNÍ NA UPLOAD IMAGE

        [HttpPost]
        public async Task<IActionResult> Create(recipeviewmodel recipe1)
        {
            if (ModelState.IsValid)
            {
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
                        Difficulty = recipe1.Difficulty,
                        CookTime = recipe1.CookTime,
                        RecipeName = recipe1.RecipeName,
                        RecipeCategory = recipe1.RecipeCategory,
                        ImageUrl = @"\images\" + uniqueFileName
                    };

                    _context.Recept.Add(r);
                    await _context.SaveChangesAsync();
                    ViewBag.Success = "Recept Přidán";
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Photo", "Please select a file.");
                }
            }

            return View(recipe1);
        }

        //[HttpPost]
        //public IActionResult Create(recipeviewmodel recipe1)
        //{
        //String filename = "";
        //if (recipe1.photo == null)
        //{
        //    String uploadfolder = Path.Combine(hostingenvironment.WebRootPath, "images");
        //    filename = Guid.NewGuid().ToString() + "_" + recipe1.photo.FileName;
        //    String filepath = Path.Combine(uploadfolder, filename);
        //    recipe1.photo.CopyTo(new FileStream(filepath, FileMode.Create));
        //    //using (var fileStream = new FileStream(filepath, FileMode.Create))
        //    //{
        //    //    recipe1.photo.CopyTo(fileStream);
        //    //}
        //}
        //Recept r = new Recept
        //{
        //    Id = recipe1.Id,
        //    Difficulty = recipe1.Difficulty,
        //    CookTime = recipe1.CookTime,
        //    RecipeName = recipe1.RecipeName,
        //    RecipeCategory = recipe1.RecipeCategory,
        //    ImageUrl = filename
        //};
        //_context.Recept.Add(r);
        //_context.SaveChanges();
        //ViewBag.success = "Recept Přidán";
        //return View();
        //}

        // POST: Recepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Difficulty,CookTime,RecipeName,RecipeCategory")] Recept recept)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(recept);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(recept);
        //}

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
                // ...
                // For example:
                Difficulty = existingRecept.Difficulty,
                CookTime = existingRecept.CookTime,
                RecipeName = existingRecept.RecipeName,
                RecipeCategory = existingRecept.RecipeCategory,
                // ...

                // Initialize PhotoModel with null or any default values as needed
                photo = null
            };

            return View(viewModel);
        }

        // POST: Recepts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                }

                _context.Update(existingRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(recipes);
        }
        /*
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

                    string ImageUrl = @"\images\" + uniqueFileName;
                    // Update other properties
                    existingRecipe.Difficulty = recipes.Difficulty;
                    existingRecipe.CookTime = recipes.CookTime;
                    existingRecipe.RecipeName = recipes.RecipeName;
                    existingRecipe.RecipeCategory = recipes.RecipeCategory;
                    existingRecipe.ImageUrl = ImageUrl;
                }

                

                _context.Update(existingRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(recipes);
        }
      */
        /* public async Task<IActionResult> Edit(Recipes recipes)
         {

             if (ModelState.IsValid)
             {
                 var existingRecipe = await _context.Recept.FindAsync(recipes.Id);
                 if (existingRecipe == null)
                 {
                     return NotFound();
                 }

                 // If the user changed the ImageUrl and it's not already prefixed with "\images\"
                 if (existingRecipe.ImageUrl != recipes.ImageUrl && !recipes.ImageUrl.StartsWith("\\images\\"))
                 {
                     recipes.ImageUrl = "\\images\\" + recipes.ImageUrl;
                 }

                 // Map properties from viewModel to existingRecipe
                 existingRecipe.Difficulty = recipes.Difficulty;
                 existingRecipe.CookTime = recipes.CookTime;
                 existingRecipe.RecipeName = recipes.RecipeName;
                 existingRecipe.RecipeCategory = recipes.RecipeCategory;
                 existingRecipe.ImageUrl = recipes.ImageUrl;

                 _context.Update(existingRecipe);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }

             return View(recipes);
         }*/
        /*
        public async Task<IActionResult> Edit(int id, [Bind("Id,Difficulty,CookTime,RecipeName,RecipeCategory,ImageUrl")] Recipes recipes)
        {
            if (id != recipes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceptExists(recipes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recipes);
        }
        */
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
