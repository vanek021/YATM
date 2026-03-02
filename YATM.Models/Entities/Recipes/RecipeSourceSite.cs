using YATM.Core.Models;

namespace YATM.Models.Entities.Recipes
{
    public class RecipeSourceSite : BaseRecord
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
    }
}
