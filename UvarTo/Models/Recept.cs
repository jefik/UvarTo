namespace UvarTo.Models
{
    public class Recept
    {
        public int Id { get; set; }
        public string Difficulty { get; set; }
        public string CookTime { get; set; }
        public string RecipeName { get; set; }
        public string RecipeCategory { get; set; }

        public Recept()
        {
            
        }
    }
}
