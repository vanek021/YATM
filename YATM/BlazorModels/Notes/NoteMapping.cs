using AutoMapper;
using YATM.Models.Entities.Notes;

namespace YATM.BlazorModels.Notes
{
    public class NoteMapping : Profile
    {
        public NoteMapping()
        {
            CreateMap<Note, NoteBlazorModel>();
            CreateMap<NoteBlazorModel, Note>()
                .ForMember(dest => dest.NoteTags, opt => opt.Ignore())
                .ForMember(dest => dest.NoteNoteTags, opt => opt.Ignore());
        }
    }
}
