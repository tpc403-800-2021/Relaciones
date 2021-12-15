using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Relaciones.Context;
using Relaciones.Models;

namespace Relaciones.Controllers
{
    public class AsignaturaEstudiantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsignaturaEstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AsignaturaEstudiantes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AsignaturaEstudiantes.Include(a => a.Asignatura).Include(a => a.Estudiante);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AsignaturaEstudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignaturaEstudiante = await _context.AsignaturaEstudiantes
                .Include(a => a.Asignatura)
                .Include(a => a.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asignaturaEstudiante == null)
            {
                return NotFound();
            }

            return View(asignaturaEstudiante);
        }

        // GET: AsignaturaEstudiantes/Create
        public IActionResult Create()
        {
            ViewData["AsignaturasId"] = new SelectList(_context.Asignaturas, "Id", "Nombre");
            ViewData["EstudiantesId"] = new SelectList(_context.Estudiantes, "Id", "Nombre");
            return View();
        }

        // POST: AsignaturaEstudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AsignaturasId,EstudiantesId")] AsignaturaEstudiante asignaturaEstudiante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asignaturaEstudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AsignaturasId"] = new SelectList(_context.Asignaturas, "Id", "Id", asignaturaEstudiante.AsignaturasId);
            ViewData["EstudiantesId"] = new SelectList(_context.Estudiantes, "Id", "Id", asignaturaEstudiante.EstudiantesId);
            return View(asignaturaEstudiante);
        }

        // GET: AsignaturaEstudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignaturaEstudiante = await _context.AsignaturaEstudiantes.FindAsync(id);
            if (asignaturaEstudiante == null)
            {
                return NotFound();
            }
            ViewData["AsignaturasId"] = new SelectList(_context.Asignaturas, "Id", "Nombre", asignaturaEstudiante.AsignaturasId);
            ViewData["EstudiantesId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", asignaturaEstudiante.EstudiantesId);
            return View(asignaturaEstudiante);
        }

        // POST: AsignaturaEstudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AsignaturasId,EstudiantesId")] AsignaturaEstudiante asignaturaEstudiante)
        {
            if (id != asignaturaEstudiante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asignaturaEstudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsignaturaEstudianteExists(asignaturaEstudiante.Id))
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
            ViewData["AsignaturasId"] = new SelectList(_context.Asignaturas, "Id", "Id", asignaturaEstudiante.AsignaturasId);
            ViewData["EstudiantesId"] = new SelectList(_context.Estudiantes, "Id", "Id", asignaturaEstudiante.EstudiantesId);
            return View(asignaturaEstudiante);
        }

        // GET: AsignaturaEstudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignaturaEstudiante = await _context.AsignaturaEstudiantes
                .Include(a => a.Asignatura)
                .Include(a => a.Estudiante)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asignaturaEstudiante == null)
            {
                return NotFound();
            }

            return View(asignaturaEstudiante);
        }

        // POST: AsignaturaEstudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asignaturaEstudiante = await _context.AsignaturaEstudiantes.FindAsync(id);
            _context.AsignaturaEstudiantes.Remove(asignaturaEstudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsignaturaEstudianteExists(int id)
        {
            return _context.AsignaturaEstudiantes.Any(e => e.Id == id);
        }
    }
}
