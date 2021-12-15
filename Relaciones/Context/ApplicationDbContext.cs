using Microsoft.EntityFrameworkCore;
using Relaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relaciones.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> _options) : base(_options)
        {

        }

        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }//select * from estudiantes
        public DbSet<Asignatura> Asignaturas { get; set; }

        public DbSet<AsignaturaEstudiante> AsignaturaEstudiantes { get; set; }

    }
}
