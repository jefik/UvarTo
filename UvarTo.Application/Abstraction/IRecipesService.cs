using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvarTo.Domain.Entities;

namespace UvarTo.Application.Abstraction
{
    public interface IRecipesService
    {
        Task<List<Recipes>> GetAllRecipes();
        Task<Recipes> GetRecipeById(int id);
        public string GetCurrentId();
        List<Recipes> GetUserRecipes(string userId);
        Task AddRecipe(recipeviewmodel recipes);
        Task <recipeviewmodel>GetRecipeForEdit(int? id);
        Task UpdateRecipe(recipeviewmodel recipes);

        Task<bool> DeleteRecipe(string userId, int id);



    }
}
