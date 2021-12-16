using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Models
{
    public class EstudianteDetails
    {
        public Estudiante Estudiante { get; set; }
        public IEnumerable<Asignatura> Asignaturas { get; set; }
    }
}
