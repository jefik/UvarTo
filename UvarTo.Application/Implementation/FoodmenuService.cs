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
    public class FoodmenuService : IFoodmenuService
    {
        private readonly ApplicationDbContext _context;
        IHostingEnvironment hostingenvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FoodmenuService(ApplicationDbContext context, IHostingEnvironment hc, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            hostingenvironment = hc;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Foodmenu>> GetAllFoodmenus()
        {
            return await _context.Foodmenu.ToListAsync();
        }

        public async Task<Foodmenu> GetFoodmenuById(int id)
        {
            return await _context.Foodmenu.FirstOrDefaultAsync(m => m.id == id);
        }

        public string GetCurrentId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        public List<Foodmenu> UserItems(string userId)
        {
            var userItems = _context.Foodmenu.Where(item => item.userId == userId).ToList();
            return userItems;
        }

        // Tips/Create
        public async Task AddFoodmenu(Foodmenu foodmenus)
        {
            var userId = GetCurrentId();

            Foodmenu f = new Foodmenu
            {

                id = foodmenus.id,
                userId = userId,
                FoodmenuName = foodmenus.FoodmenuName,
                FoodmenuText = foodmenus.FoodmenuText
            };

            _context.Foodmenu.Add(f);
            await _context.SaveChangesAsync();

        }

        // Tips/Edit
        public async Task<bool> UpdateFoodmenu(Foodmenu foodmenus)
        {
            var existingFoodmenu = await _context.Foodmenu.FindAsync(foodmenus.id);
            if (existingFoodmenu == null)
            {
                return false;
            }

            existingFoodmenu.FoodmenuName = foodmenus.FoodmenuName;
            existingFoodmenu.FoodmenuText = foodmenus.FoodmenuText;

            _context.Update(existingFoodmenu);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFood(string userId, int Id)
        {

            var foodmenus = await _context.Foodmenu
                .FirstOrDefaultAsync(s => s.userId == userId && s.id == Id);

            if (foodmenus == null)
            {
                return false; 
            }

       
            _context.Foodmenu.Remove(foodmenus);
            await _context.SaveChangesAsync();

            return true; 
        }

        //public async Task<bool> DeleteFoodmenu(int id)
        //{
        //    var foodmenus = await _context.Foodmenu.FindAsync(id);
        //    if (foodmenus == null)
        //    {
        //        return false;
        //    }

        //    _context.Foodmenu.Remove(foodmenus);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
    
}
