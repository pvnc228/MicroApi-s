namespace NotesService.Models
{
    public record NoteModel(int Id, string Username, string Title, string Content, DateTime CreatedAt, DateTime? UpdatedAt = null);

}
