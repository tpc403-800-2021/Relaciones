﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public virtual ICollection<Asignatura> Asignaturas { get; set; }
    }
}
