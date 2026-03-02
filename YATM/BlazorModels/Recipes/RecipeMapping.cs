using AutoMapper;
using YATM.Models.Entities.Recipes;

namespace YATM.BlazorModels.Recipes
{
    public class RecipeMapping : Profile
    {
        public RecipeMapping()
        {
            CreateMap<Recipe, RecipeBlazorModel>()
                .ForMember(dest => dest.SourceSiteName, opt => opt.MapFrom(src => src.SourceSite != null ? src.SourceSite.Name : null))
                .ForMember(dest => dest.SourceSiteHost, opt => opt.MapFrom(src => src.SourceSite != null ? src.SourceSite.Host : null));

            CreateMap<RecipeBlazorModel, Recipe>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.SourceSite, opt => opt.Ignore())
                .ForMember(dest => dest.SourceSiteId, opt => opt.Ignore())
                .ForMember(dest => dest.SourceUrl, opt => opt.Ignore());
        }
    }
}
