using NotesService.Models;

namespace NotesService.Interface
{
    public interface INotesService
    {
        Task<(bool Success, string Message)> CreateNoteAsync(NoteModel model);
        Task<(bool Success, string Message)> UpdateNoteAsync(NoteModel model);
        Task<(bool Success, string Message)> DeleteNoteAsync(int noteId, string username);
        Task<IEnumerable<NoteModel>> RetrieveNotesAsync(string username);
    }
}
