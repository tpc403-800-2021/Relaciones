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
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            var viewModel = new EstudiantesIndex();
            viewModel.Estudiantes = await _context.Estudiantes
                .Include(e => e.AsignaturaEstudiantes)
                .ThenInclude(ae => ae.Asignatura)
                .AsNoTracking()
                .ToListAsync();

            //return View(await _context.Estudiantes.ToListAsync());
            return View(viewModel);
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*
            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(m => m.Id == id);
            */
            var viewModel = new EstudianteDetails();
            viewModel.Estudiante = await _context.Estudiantes
                .Include(e => e.AsignaturaEstudiantes)
                .ThenInclude(ae => ae.Asignatura)
                .FirstAsync(e => e.Id == id);

            if (viewModel.Estudiante == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            Estudiante estudiante = new Estudiante();
            estudiante.AsignaturaEstudiantes = new List<AsignaturaEstudiante>();

            CargarListaDeAsignaturasAsignadas(estudiante);

            return View();
        }

        private void CargarListaDeAsignaturasAsignadas(Estudiante estudiante)
        {
            var todasLasAsignaturas = _context.Asignaturas;
            var estudianteAsignaturas = new HashSet<int>(estudiante.AsignaturaEstudiantes.Select(ae => ae.AsignaturasId));
            var viewModel = new List<AssignedAsignatura>();
            foreach (var asignatura in todasLasAsignaturas)
            {
                viewModel.Add(new AssignedAsignatura
                {
                    AsignaturaId = asignatura.Id,
                    AsignaturaNombre = asignatura.Nombre,
                    isAsignado = estudianteAsignaturas.Contains(asignatura.Id)
                });
            }
            ViewData["Asignaturas"] = viewModel;

        }

        // POST: Estudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Edad")] Estudiante estudiante, string[] asignaturasSeleccionadas)
        {
            if(asignaturasSeleccionadas != null)
            {
                estudiante.AsignaturaEstudiantes = new List<AsignaturaEstudiante>();

                foreach(var asignatura in asignaturasSeleccionadas) //asignatura guarda el id de la asignatura pero en formato string
                {
                    var asignaturaAAsignar = new AsignaturaEstudiante
                    {
                        EstudiantesId = estudiante.Id,
                        AsignaturasId = Convert.ToInt32(asignatura)
                    };
                    estudiante.AsignaturaEstudiantes.Add(asignaturaAAsignar);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var estudiante = await _context.Estudiantes.FindAsync(id);
            var estudiante = await _context.Estudiantes
                .Include(e => e.AsignaturaEstudiantes)
                .ThenInclude(ae => ae.Asignatura)
                .AsNoTracking()
                .FirstAsync(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            CargarListaDeAsignaturasAsignadas(estudiante);
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] asignaturasSeleccionadas)
        {
            /*
            if (id != estudiante.Id)
            {
                return NotFound();
            }
            */

            if(id == null)
            {
                return NotFound();
            }

            var estudianteParaEditar = await _context.Estudiantes
                .Include(e => e.AsignaturaEstudiantes)
                .ThenInclude(ae => ae.Asignatura)
                .FirstAsync(e => e.Id == id);

            if(await TryUpdateModelAsync<Estudiante>(estudianteParaEditar, "", e => e.Nombre, e => e.Edad))
            {

                ActualizarRelacionesAsignaturaEstudiante(estudianteParaEditar, asignaturasSeleccionadas);

                try
                {
                    await _context.SaveChangesAsync();
                }catch(DbUpdateException e)
                {
                    //Mensaje de error en caso de que no pueda hacer el update
                    ModelState.AddModelError("error", "descripción del error");
                }

                return RedirectToAction(nameof(Index));
            }
            CargarListaDeAsignaturasAsignadas(estudianteParaEditar);
            return View(estudianteParaEditar);

            /*
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.Id))
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
            return View(estudiante);
            */
        }

        private void ActualizarRelacionesAsignaturaEstudiante(Estudiante estudianteParaEditar, string[] asignaturasSeleccionadas)
        {
            if (asignaturasSeleccionadas == null)
            {
                estudianteParaEditar.AsignaturaEstudiantes = new List<AsignaturaEstudiante>();

            }

            var asignaturasSeleccionadasHS = new HashSet<string>(asignaturasSeleccionadas); //las asignaturas que vienen del formulario (los id en str)
            var asignaturaEstudianteHS = new HashSet<int>(estudianteParaEditar.AsignaturaEstudiantes.Select(ae => ae.Asignatura.Id)); //las asignaturas actuales del estudiantes(los id en int)

            foreach (var asignatura in _context.Asignaturas)
            {
                if (asignaturasSeleccionadasHS.Contains(asignatura.Id.ToString()))
                {
                    if (!asignaturaEstudianteHS.Contains(asignatura.Id))
                    {
                        estudianteParaEditar.AsignaturaEstudiantes.Add(new AsignaturaEstudiante
                        {
                            AsignaturasId = asignatura.Id,
                            EstudiantesId = estudianteParaEditar.Id
                        });
                    }
                    /*
                    else
                    {
                    //Ésto se ejecuta si el estudiante ya posee la relación con la asignatura en la tabla intermedia
                    }
                    */
                }
                else
                {
                    if (asignaturaEstudianteHS.Contains(asignatura.Id))
                    {
                        AsignaturaEstudiante relacionAsEsARemover = estudianteParaEditar.AsignaturaEstudiantes.First(ae => ae.AsignaturasId == asignatura.Id);
                        _context.Remove(relacionAsEsARemover);
                    }

                }
            }
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}
