using AutoMapper;
using YATM.Models.Entities.Recipes;

namespace YATM.BlazorModels.Recipes
{
    public class RecipeMapping : Profile
    {
        public RecipeMapping()
        {
            CreateMap<Recipe, RecipeBlazorModel>()
                .ForMember(dest => dest.SourceName,
                    opt => opt.MapFrom(src => src.RecipeSource != null ? src.RecipeSource.Name : string.Empty))
                .ForMember(dest => dest.SourceHost,
                    opt => opt.MapFrom(src => src.RecipeSource != null ? src.RecipeSource.Host : string.Empty))
                .ForMember(dest => dest.ImportedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
