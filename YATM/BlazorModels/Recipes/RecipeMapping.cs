using AutoMapper;
using YATM.Models.Entities.Recipes;

namespace YATM.BlazorModels.Recipes
{
    public class RecipeMapping : Profile
    {
        public RecipeMapping()
        {
            CreateMap<Recipe, RecipeBlazorModel>();
            CreateMap<RecipeBlazorModel, Recipe>()
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
