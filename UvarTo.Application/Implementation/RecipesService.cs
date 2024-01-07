using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using UvarTo.Application.Abstraction;
using UvarTo.Domain.Entities;
using UvarTo.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UvarTo.Application.Implementation
{
    public class RecipesService : IRecipesService
    {
        private readonly ApplicationDbContext _context;
        IHostingEnvironment hostingenvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RecipesService(ApplicationDbContext context, IHostingEnvironment hc, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            hostingenvironment = hc;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Recipes>> GetAllRecipes()
        {
            return await _context.Recept.ToListAsync();
        }

        public async Task<Recipes> GetRecipeById(int id)
        {
            return await _context.Recept.FirstOrDefaultAsync(m => m.Id == id);
        }
        public string GetCurrentId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        public List<Recipes> GetUserRecipes(string userId)
        {
            var userItems = _context.Recept.Where(item => item.userId == userId).ToList();
            return userItems;
        }

        // GET: Recipes/Create
        public async Task AddRecipe(recipeviewmodel recipes)
        {
            var userId = GetCurrentId();
            if (recipes.photo != null)
            {
                string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + recipes.photo.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await recipes.photo.CopyToAsync(stream);
                }

                Recipes r = new Recipes
                {
                    // Set other properties here
                    Id = recipes.Id,
                    userId = userId,
                    Difficulty = recipes.Difficulty,
                    CookTime = recipes.CookTime,
                    RecipeName = recipes.RecipeName,
                    RecipeCategory = recipes.RecipeCategory,
                    RecipeText = recipes.RecipeText,
                    ImageUrl = @"\images\" + uniqueFileName
                };

                _context.Recept.Add(r);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Please, choose a file.");
            }
        }
        // GET: Recepts/Edit/5
        public async Task<recipeviewmodel> GetRecipeForEdit(int? id)
        {
            var existingRecipe = await _context.Recept.FindAsync(id);
            if (existingRecipe == null)
            {
                throw new ArgumentNullException(nameof(existingRecipe), "Recipes Added.");
            }

            var viewModel = new recipeviewmodel
            {
                Id = existingRecipe.Id,
                Difficulty = existingRecipe.Difficulty,
                CookTime = existingRecipe.CookTime,
                RecipeName = existingRecipe.RecipeName,
                RecipeCategory = existingRecipe.RecipeCategory,
                RecipeText = existingRecipe.RecipeText,
                photo = null // Initialize PhotoModel with null or any default values as needed
            };

            return viewModel;
        }

        // POST: Recepts/Edit/5

        public async Task UpdateRecipe(recipeviewmodel recipes)
        {
            var existingRecipe = await _context.Recept.FindAsync(recipes.Id);
            if (existingRecipe == null)
            {
                throw new ArgumentNullException(nameof(existingRecipe), "Recipe wasn't found.");
            }

            if (recipes.photo != null)
            {
                string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + recipes.photo.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

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
        }

        // GET: Recipes/Delete/5
        public async Task<bool> DeleteRecipe(int id)
        {
            var recept = await _context.Recept.FindAsync(id);
            if (recept != null)
            {
                _context.Recept.Remove(recept);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
