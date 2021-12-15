using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Models
{
    public class AssignedAsignatura
    {
        public int AsignaturaId { get; set; }
        public string AsignaturaNombre { get; set; }
        public bool isAsignado { get; set; }
    }
}
