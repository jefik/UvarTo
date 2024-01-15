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

namespace UvarTo.Application.Implementation
{
    public class SearchRService : ISearchRService
    {
        private readonly ApplicationDbContext _context;

        public SearchRService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recipes>> GetRSearchResults(string searchPhrase)
        {
            if (string.IsNullOrEmpty(searchPhrase))
            {
                return await _context.Recept.ToListAsync();
            }
            else
            {
                return await _context.Recept
                    .Where(j => j.RecipeText.Contains(searchPhrase))
                    .ToListAsync();
            }
        }

    }
}
