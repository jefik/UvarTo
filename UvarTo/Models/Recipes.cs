namespace UvarTo.Models
{
    public class Recipes
    {
        public int Id { get; set; }
        public string Difficulty { get; set; }
        public string CookTime { get; set; }
        public string RecipeName { get; set; }
        public string RecipeText { get; set; }
        public string RecipeCategory { get; set; }
        public string ImageUrl { get; set; }
        public Recipes()
        {
            
        }
    }
}
