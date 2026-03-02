namespace YATM.BlazorModels.Recipes
{
    public class RecipeImportSourceSiteBlazorModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<string> Hosts { get; set; } = new();
    }
}
