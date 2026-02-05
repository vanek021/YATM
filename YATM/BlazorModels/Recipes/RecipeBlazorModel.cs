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
    }
}
