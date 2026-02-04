using AutoMapper;
using YATM.Models.Entities;

namespace YATM.BlazorModels.Users
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserShortBlazorModel>();
        }
    }
}
