using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvarTo.Domain.Entities;
using UvarTo.Infrastructure.Database;
using UvarTo.Application.Abstraction;
using UvarTo.Domain.Entities;
using UvarTo.Infrastructure.Database;

namespace UvarTo.Application.Implementation
{
    public class SearchFService : ISearchFService
    {
        private readonly ApplicationDbContext _context;

        public SearchFService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Foodmenu>> GetFSearchResults(string searchPhrase)
        {
            if (string.IsNullOrEmpty(searchPhrase))
            {
                return await _context.Foodmenu.ToListAsync();
            }
            else
            {
                return await _context.Foodmenu
                    .Where(j => j.FoodmenuName.Contains(searchPhrase))
                    .ToListAsync();
            }
        }

    }
}
