using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Models
{
    public class Funcionario
    {
        private int _id;
        private string _nombre;
        private int _idRol;

        [Key]
        public int Id { get => _id; set => _id = value; }
        [StringLength(255)]
        [Required]
        public string Nombre { get => _nombre; set => _nombre = value; }

        [ForeignKey("Rol")]
        [DisplayName("Roles")]
        public int IdRol { get => _idRol; set => _idRol = value; }

        public virtual Rol Rol { get; set; }
    }
}
