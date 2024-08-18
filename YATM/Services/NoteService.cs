using AntDesign;
using AutoMapper;
using YATM.BlazorModels.Notes;
using YATM.BlazorModels.Notes.NoteTags;
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

        public async Task<List<NoteBlazorModel>> GetNoteBlazorModelsByUserAsync(User user, List<long>? tagIds = null)
        {
            var notes = await _db.Notes.GetAllNotesByUser(user, tagIds);

            return _mapper.Map<List<NoteBlazorModel>>(notes);
        }

        public async Task<Note> CreateNoteAsync(NoteBlazorModel model)
        {
            var note = _mapper.Map<Note>(model);
            var noteTags = await _db.NoteTags.GetAllAsync();

            note.UserId = _appCtx.CurrentUser.Id;
            note.NoteTags = noteTags.Where(nt => model.NoteTagsIds.Contains(nt.Id)).ToList();

            _db.Notes.Insert(note);
            await _db.SaveChangesAsync();

            return note;
        }

        public async Task DeleteNoteAsync(long id)
        {
            _db.Notes.Delete(id);
            await _db.SaveChangesAsync();
        }

        public async Task PinNoteAsync(NoteBlazorModel model)
        {
            var note = await _db.Notes.GetByIdAsync(model.Id);
            note!.IsPinned = model.IsPinned;
            _db.Notes.Update(note);
            await _db.SaveChangesAsync();
        }

        public async Task<Note> UpdateNoteAsync(NoteBlazorModel model)
        {
            if (model.Id == default)
                throw new ArgumentException();

            var note = await _db.Notes.GetByIdAsync(model.Id);
            var noteTags = await _db.NoteTags.GetAllAsync();

            _mapper.Map(model, note);
            note!.NoteTags.Clear();
            _db.Notes.Update(note!);
            await _db.SaveChangesAsync();

            note!.NoteTags = noteTags.Where(nt => model.NoteTagsIds.Contains(nt.Id)).ToList();
            _db.Notes.Update(note!);
            await _db.SaveChangesAsync();

            return note!;
        }

        public async Task<List<NoteTagBlazorModel>> GetNoteTags()
        {
            var tags = await _db.NoteTags.GetAllAsync();
            return _mapper.Map<List<NoteTagBlazorModel>>(tags);
        }

        public async Task<List<NoteTagBlazorModel>> GetNoteTagsWithNotes()
        {
            var tags = await _db.NoteTags.GetAllWithNotesAsync();
            return _mapper.Map<List<NoteTagBlazorModel>>(tags);
        }
    }
}
