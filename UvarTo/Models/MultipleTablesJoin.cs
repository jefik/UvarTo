namespace UvarTo.Models
{
    public class MultipleTablesJoin
    {
        public List<Recipes> recipeList { get; set; }
        public List<Tips> tipsList { get; set; }
        public List<Foodmenu> foodList { get; set; }
        //public IEnumerable<Recipes> recipeList { get; set; }
        //public IEnumerable<Foodmenu> tipsList { get; set; }
        //public IEnumerable<Tips> foodList { get; set; }

    }
}
