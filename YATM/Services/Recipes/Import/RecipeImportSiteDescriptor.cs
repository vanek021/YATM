namespace YATM.Services.Recipes.Import
{
    public class RecipeImportSiteDescriptor
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<string> Hosts { get; set; } = new();
    }
}
