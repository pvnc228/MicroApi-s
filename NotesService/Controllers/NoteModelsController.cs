using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotesService.Context;
using NotesService.Models;

namespace NotesService.Controllers
{
    public class NoteModelsController : Controller
    {
        private readonly NotesContext _context;

        public NoteModelsController(NotesContext context)
        {
            _context = context;
        }

        // GET: NoteModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.NoteModel.ToListAsync());
        }

        // GET: NoteModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteModel = await _context.NoteModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteModel == null)
            {
                return NotFound();
            }

            return View(noteModel);
        }

        // GET: NoteModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NoteModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Title,Content,CreatedAt,UpdatedAt")] NoteModel noteModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noteModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(noteModel);
        }

        // GET: NoteModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteModel = await _context.NoteModel.FindAsync(id);
            if (noteModel == null)
            {
                return NotFound();
            }
            return View(noteModel);
        }

        // POST: NoteModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Title,Content,CreatedAt,UpdatedAt")] NoteModel noteModel)
        {
            if (id != noteModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noteModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteModelExists(noteModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(noteModel);
        }

        // GET: NoteModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteModel = await _context.NoteModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteModel == null)
            {
                return NotFound();
            }

            return View(noteModel);
        }

        // POST: NoteModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var noteModel = await _context.NoteModel.FindAsync(id);
            if (noteModel != null)
            {
                _context.NoteModel.Remove(noteModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteModelExists(int id)
        {
            return _context.NoteModel.Any(e => e.Id == id);
        }
    }
}
