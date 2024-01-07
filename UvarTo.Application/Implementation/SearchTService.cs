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
    public class SearchTService : ISearchTService
    {
        private readonly ApplicationDbContext _context;

        public SearchTService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Same as the SearchRService if we could make into one method would be really cool)
        public async Task<List<Tips>> GetTSearchResults(string searchPhrase)
        {
            return await _context.Tips
                .Where(j => j.TipName.Contains(searchPhrase))
                .ToListAsync();
        }
    }
}
