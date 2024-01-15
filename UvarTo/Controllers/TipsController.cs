using Microsoft.AspNetCore.Mvc;
using UvarTo.Domain.Entities;
using UvarTo.Application.Implementation;
using UvarTo.Application.Abstraction;
using Microsoft.EntityFrameworkCore;


namespace UvarTo.Controllers
{
    public class TipsController : Controller
    {
        private readonly ITipsService _tipsService;
        private readonly ISearchTService _searchTService;
        
        public TipsController(ISearchTService searchTService, ITipsService tipsService)
        {
            _searchTService = searchTService;
            _tipsService = tipsService;
        }

        public async Task<IActionResult> UserItems()
        {
            var userId = _tipsService.GetCurrentId();

            var userItems = _tipsService.UserItems(userId);

            ViewBag.UserIdString = userId;
            return View("userItems", userItems);
        }
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            var searchResults = await _searchTService.GetTSearchResults(searchPhrase);
            return View("Index", searchResults);
        }
        public async Task<IActionResult> Index()
        {
            var tips = await _tipsService.GetAllTips();

            if (tips != null)
            {
                return View(tips);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Tips' is null.");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tips = await _tipsService.GetTipById(id.Value);

            if (tips == null)
            {
                return NotFound();
            }

            return View(tips);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tips tips)
        {
            
                await _tipsService.AddTip(tips);
                ViewBag.Success = "Tip added";
                return RedirectToAction(nameof(Index));
            

            return View(tips);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existingTip = await _tipsService.GetTipById(id.Value);
            if (existingTip == null)
            {
                return NotFound();
            }

            var tips = new Tips
            {
                Id = existingTip.Id,
                TipName = existingTip.TipName,
                TipText = existingTip.TipText
            };

            return View(tips);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tips tips)
        {
            if (id != tips.Id)
            {
                return NotFound();
            }

           
                var result = await _tipsService.UpdateTip(tips);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            

            return View(tips);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTips(int id)
        {
            var userId = _tipsService.GetCurrentId();

            var isDeleted = await _tipsService.DeleteTip(userId, id);

            if (!isDeleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Tip deleted successfully.";
            return RedirectToAction("Index");
        }
        
    }
}
