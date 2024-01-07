using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvarTo.Domain.Entities;

namespace UvarTo.Application.Abstraction
{
    public interface ITipsService
    {
        Task<List<Tips>> GetAllTips();
        Task<Tips> GetTipById(int id);
        public string GetCurrentId();
        List<Tips> UserItems(string userId);
        Task AddTip(Tips tips);
        Task<bool> UpdateTip(Tips tips);
        Task<bool> DeleteTip(int id);
    }
}
