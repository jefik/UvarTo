using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UvarTo.Infrastructure.Identity;

namespace UvarTo.Application.Abstraction
{
    public interface IAdminService
    {
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user);

        Task<List<ApplicationUser>> GetAllUsersAsync();

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<bool> UpdateUserAsync(ApplicationUser user);

        
    }
}
