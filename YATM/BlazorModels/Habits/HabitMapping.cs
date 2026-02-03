using AutoMapper;
using YATM.Models.Entities.Habits;

namespace YATM.BlazorModels.Habits
{
    public class HabitMapping : Profile
    {
        public HabitMapping()
        {
            CreateMap<Habit, HabitBlazorModel>();
            CreateMap<HabitBlazorModel, Habit>()
                .ForMember(dest => dest.CheckIns, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<HabitCheckIn, HabitCheckInBlazorModel>();
            CreateMap<HabitCheckInBlazorModel, HabitCheckIn>()
                .ForMember(dest => dest.Habit, opt => opt.Ignore());
        }
    }
}
