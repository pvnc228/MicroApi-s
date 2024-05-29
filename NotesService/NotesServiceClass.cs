using Microsoft.EntityFrameworkCore;
using NotesService.Context;
using NotesService.Interface;
using NotesService.Models;

namespace NotesService.Class
{
    public class NotesServiceClass : INotesService
    {
        private readonly NotesContext _context;

        public NotesServiceClass(NotesContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> CreateNoteAsync(NoteModel model)
        {
            try
            {
                var storedNote = new StoredNote
                {
                    Username = model.Username,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedAt = DateTime.UtcNow
                };

                _context.StoredNotes.Add(storedNote);
                await _context.SaveChangesAsync();

                return (true, "Note created successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to create note: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateNoteAsync(NoteModel model)
        {
            try
            {
                var storedNote = await _context.StoredNotes.FindAsync(model.Id);
                if (storedNote == null || storedNote.Username != model.Username)
                {
                    return (false, "Note not found or access denied.");
                }

                storedNote.Title = model.Title;
                storedNote.Content = model.Content;
                storedNote.UpdatedAt = DateTime.UtcNow;

                _context.StoredNotes.Update(storedNote);
                await _context.SaveChangesAsync();

                return (true, "Note updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to update note: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteNoteAsync(int noteId, string username)
        {
            try
            {
                var storedNote = await _context.StoredNotes.FindAsync(noteId);
                if (storedNote == null || storedNote.Username != username)
                {
                    return (false, "Note not found or access denied.");
                }

                _context.StoredNotes.Remove(storedNote);
                await _context.SaveChangesAsync();

                return (true, "Note deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Failed to delete note: {ex.Message}");
            }
        }

        public async Task<IEnumerable<NoteModel>> RetrieveNotesAsync(string username)
        {
            var notes = await _context.StoredNotes
                .Where(n => n.Username == username)
                .Select(n => new NoteModel(n.Id, n.Username, n.Title, n.Content, n.CreatedAt, n.UpdatedAt))
                .ToListAsync();

            return notes;
        }
    }
}
