using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvarTo.Domain.Entities;

namespace UvarTo.Application.Abstraction
{
    public interface ISearchFService
    {
        Task<List<Foodmenu>> GetFSearchResults(string searchPhrase);

    }
}
