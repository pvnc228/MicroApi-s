using Microsoft.AspNetCore.Mvc;
using NotesService.Interface;
using NotesService.Models;

namespace NotesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNoteAsync(NoteModel model)
        {
            var result = await _notesService.CreateNoteAsync(model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateNoteAsync(NoteModel model)
        {
            var result = await _notesService.UpdateNoteAsync(model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpDelete("delete/{noteId}")]
        public async Task<IActionResult> DeleteNoteAsync(int noteId, string username)
        {
            var result = await _notesService.DeleteNoteAsync(noteId, username);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("retrieve")]
        public async Task<IActionResult> RetrieveNotesAsync(string username)
        {
            var notes = await _notesService.RetrieveNotesAsync(username);
            if (notes == null || !notes.Any())
            {
                return NotFound();
            }
            return Ok(notes);
        }
    }
}
