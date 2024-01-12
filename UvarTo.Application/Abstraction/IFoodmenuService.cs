using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvarTo.Domain.Entities;

namespace UvarTo.Application.Abstraction
{
    public interface IFoodmenuService
    {
        Task<List<Foodmenu>> GetAllFoodmenus();
        Task<Foodmenu> GetFoodmenuById(int id);
        public string GetCurrentId();
        List<Foodmenu> UserItems(string userId);
        Task AddFoodmenu(Foodmenu foodmenus);
        Task<bool> UpdateFoodmenu(Foodmenu foodmenus);
        Task<bool> DeleteFood(string userId, int Id);

    }
}
