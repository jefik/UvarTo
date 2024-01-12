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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UvarTo.Application.Implementation
{
    public class TipsService : ITipsService
    {
        private readonly ApplicationDbContext _context;
        IHostingEnvironment hostingenvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TipsService(ApplicationDbContext context, IHostingEnvironment hc, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            hostingenvironment = hc;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Tips>> GetAllTips()
        {
            return await _context.Tips.ToListAsync();
        }

        public async Task<Tips> GetTipById(int id)
        {
            return await _context.Tips.FirstOrDefaultAsync(m => m.Id == id);
        }

        public string GetCurrentId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        public List<Tips> UserItems(string userId)
        {
            var userItems = _context.Tips.Where(item => item.userId == userId).ToList();
            return userItems;
        }

        // Tips/Create
        public async Task AddTip(Tips tips)
        {
            var userId = GetCurrentId();
            
                Tips t = new Tips
                {

                    Id = tips.Id,
                    userId = userId,
                    TipName = tips.TipName,
                    TipText = tips.TipText
                };

                _context.Tips.Add(t);
                await _context.SaveChangesAsync();
            
        }

        // Tips/Edit
        public async Task<bool> UpdateTip(Tips tips)
        {
            var existingTips = await _context.Tips.FindAsync(tips.Id);
            if (existingTips == null)
            {
                return false;
            }

            existingTips.TipName = tips.TipName;
            existingTips.TipText = tips.TipText;

            _context.Update(existingTips);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTip(string userId, int Id)
        {

            var tips = await _context.Tips
                .FirstOrDefaultAsync(s => s.userId == userId && s.Id == Id);

            if (tips == null)
            {
                return false;
            }


            _context.Tips.Remove(tips);
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> DeleteTip(int id)
        //{
        //    var tips = await _context.Tips.FindAsync(id);
        //    if (tips == null)
        //    {
        //        return false;
        //    }

        //    _context.Tips.Remove(tips);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}
