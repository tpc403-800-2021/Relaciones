using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Models
{
    public class Rol
    {
        private int _id;
        private string _nombre;

        [Key]
        public int Id { get => _id; set => _id = value; }
        [Required]
        [StringLength(255)]
        public string Nombre { get => _nombre; set => _nombre = value; }

        public virtual ICollection<Funcionario> Funcionarios { get; set; }
    }
}
