using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvarTo.Domain.Entities
{
    public class Together
    {
        public List<Recipes> Recipes { get; set; }
        public List<Tips> Tips { get; set; }
        public List<Foodmenu> Foodmenus { get; set; }
        public string userName { get; set; }

        public Together()
        {
            Recipes = new List<Recipes>();
            Tips = new List<Tips>();
            Foodmenus = new List<Foodmenu>();
        }
    }
}