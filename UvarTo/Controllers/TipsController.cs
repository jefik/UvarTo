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
    public class TipsController : Controller
    {
        private readonly ApplicationDbContext _context;
        IWebHostEnvironment hostingenvironment;

        public TipsController(ApplicationDbContext context, IWebHostEnvironment hc)
        {
            _context = context;
            hostingenvironment = hc;
        }

        // GET: Tips
        public async Task<IActionResult> Index()
        {
            return _context.Tips != null ?
                        View(await _context.Tips.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Tips'  is null.");
        }

       
        // GET: Tips/ShowSearchForm
        public IActionResult Search()
        {
            return View("Index");
        }
        // POST: Tips/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var searchResults = await _context.Tips
            .Where(j => j.TipName.Contains(SearchPhrase))
            .ToListAsync();

            return View("Index", searchResults);
        }
        // GET: Tips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tips == null)
            {
                return NotFound();
            }

            var tips = await _context.Tips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tips == null)
            {
                return NotFound();
            }

            return View(tips);
        }

        // GET: Tips/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tips tips)
        {
            if (ModelState.IsValid)
            {

                    Tips t = new Tips
                    {
                        // Set other properties here
                        Id = tips.Id,
                        TipName = tips.TipName,
                        TipText = tips.TipText,
                    };

                    _context.Tips.Add(t);
                    await _context.SaveChangesAsync();
                    ViewBag.Success = "Tip Added";
                    return RedirectToAction(nameof(Index));

            }

            return View(tips);
        }

        // GET: Tips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tips == null)
            {
                return NotFound();
            }

            var existingTip = await _context.Tips.FindAsync(id);
            if (existingTip == null)
            {
                return NotFound();
            }

            var tips = new Tips
            {
                Id = existingTip.Id,
                // Assign other properties from existingTip to Tips
                
                TipName = existingTip.TipName,
                TipText = existingTip.TipText,
                                
            };

            return View(tips);
        }

        // POST: Tips/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tips tips)
        {
            if (id != tips.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingTip = await _context.Tips.FindAsync(tips.Id);
                if (existingTip == null)
                {
                    return NotFound();
                }

                
                    // Update other properties
                    existingTip.TipName = tips.TipName;
                    existingTip.TipText = tips.TipText;
                    
                

                _context.Update(existingTip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(tips);
        }
        // GET: Tips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tips == null)
            {
                return NotFound();
            }

            var tips = await _context.Tips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tips == null)
            {
                return NotFound();
            }

            return View(tips);
        }

        // POST: Tips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tips == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tips'  is null.");
            }
            var tips = await _context.Tips.FindAsync(id);
            if (tips != null)
            {
                _context.Tips.Remove(tips);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipsExists(int id)
        {
            return (_context.Tips?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
