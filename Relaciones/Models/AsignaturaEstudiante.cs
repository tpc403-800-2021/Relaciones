using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Models
{
    public class AsignaturaEstudiante
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Asignatura")]
        public int AsignaturasId { get; set; }
        [ForeignKey("Estudiante")]
        public int EstudiantesId { get; set; }

        public virtual Asignatura Asignatura { get; set; }
        public virtual Estudiante Estudiante { get; set; }
    }
}
