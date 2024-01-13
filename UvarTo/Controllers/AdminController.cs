using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using UvarTo.Infrastructure.Identity;
using UvarTo.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using UvarTo.Domain.Entities;
using UvarTo.Application.Abstraction;

namespace UvarTo.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _adminService.GetCurrentUserAsync(User);

            if (user != null)
            {
                ViewData["UserFirstName"] = user.FirstName;
            }

            var users = await _adminService.GetAllUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _adminService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var updateResult = await _adminService.UpdateUserAsync(user);

                if (updateResult)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle errors if the update fails
                    ModelState.AddModelError(string.Empty, "User not found or update failed.");
                }
            }

            // If ModelState is not valid, return to the edit view with validation errors
            return View("Edit", user);
        }
    }
}
