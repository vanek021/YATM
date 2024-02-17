using AutoMapper;
using YATM.BlazorModels.Notes;
using YATM.Data;
using YATM.Models.Entities;
using YATM.Models.Entities.Notes;

namespace YATM.Services
{
    public class NoteService
    {
        private readonly ApplicationContext _appCtx;
        private readonly Database _db;
        private readonly IMapper _mapper;

        public NoteService(ApplicationContext appCtx, Database db, IMapper mapper)
        {
            _appCtx = appCtx;
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<NoteBlazorModel>> GetNoteBlazorModelsByUserAsync(User user)
        {
            var notes = await _db.Notes.GetAllNotesByUser(user);

            return _mapper.Map<List<NoteBlazorModel>>(notes);
        }

        public async Task<Note> CreateNoteAsync(NoteBlazorModel model)
        {
            var note = _mapper.Map<Note>(model);
            note.UserId = _appCtx.CurrentUser.Id;
            _db.Notes.Insert(note);
            await _db.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNoteAsync(NoteBlazorModel model)
        {
            if (model.Id == default)
                throw new ArgumentException();

            var note = await _db.Notes.GetByIdAsync(model.Id);
            _mapper.Map(model, note);
            _db.Notes.Update(note);
            await _db.SaveChangesAsync();
            return note;
        }
    }
}
