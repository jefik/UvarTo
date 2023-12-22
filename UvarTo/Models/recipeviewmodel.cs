namespace UvarTo.Models
{
    public class recipeviewmodel
    {
        public int Id { get; set; }
        public string Difficulty { get; set; }
        public string CookTime { get; set; }
        public string RecipeName { get; set; }
        public string RecipeCategory { get; set; }
        public string RecipeText { get; set; }
        public IFormFile photo { get; set; }
    }
}
