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
using UvarTo.Infrastructure.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace UvarTo.Application.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _adminManager;
        

        public AdminService(UserManager<ApplicationUser> adminManager)
        {
            _adminManager = adminManager;

        }

        public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return await _adminManager.GetUserAsync(user);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _adminManager.Users.ToListAsync();
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _adminManager.FindByIdAsync(userId);
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await _adminManager.FindByIdAsync(user.Id);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;

            var result = await _adminManager.UpdateAsync(existingUser);

            return result.Succeeded;
        }
    }
}
