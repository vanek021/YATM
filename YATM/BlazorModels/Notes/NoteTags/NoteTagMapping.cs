using AutoMapper;
using YATM.Models.Entities.Notes;

namespace YATM.BlazorModels.Notes.NoteTags
{
    public class NoteTagMapping : Profile
    {
        public NoteTagMapping()
        {
            CreateMap<NoteTag, NoteTagBlazorModel>();
            CreateMap<NoteTagBlazorModel, NoteTag>();
        }
    }
}
