namespace YATM.Services.Recipes.Import
{
    public class ImportedRecipeModel
    {
        public string SourceCode { get; set; }
        public string SourceName { get; set; }
        public string SourceHost { get; set; }
        public string SourceUrl { get; set; }
        public string Title { get; set; }
        public string ContentHtml { get; set; }
    }
}
