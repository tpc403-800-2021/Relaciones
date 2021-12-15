using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Models
{
    public class Asignatura
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public virtual ICollection<AsignaturaEstudiante> AsignaturaEstudiantes { get; set; }
    }
}
