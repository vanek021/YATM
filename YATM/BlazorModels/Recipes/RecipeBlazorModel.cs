using System.ComponentModel.DataAnnotations;

namespace YATM.BlazorModels.Recipes
{
    public class RecipeBlazorModel
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public long? SourceSiteId { get; set; }
        public string? SourceSiteName { get; set; }
        public string? SourceSiteHost { get; set; }
        public string? SourceUrl { get; set; }
    }
}
