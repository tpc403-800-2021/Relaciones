using Microsoft.EntityFrameworkCore.Migrations;

namespace Relaciones.Migrations
{
    public partial class AddTableIntermedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsignaturaEstudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsignaturasId = table.Column<int>(type: "int", nullable: false),
                    EstudiantesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignaturaEstudiantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsignaturaEstudiantes_Asignaturas_AsignaturasId",
                        column: x => x.AsignaturasId,
                        principalTable: "Asignaturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignaturaEstudiantes_Estudiantes_EstudiantesId",
                        column: x => x.EstudiantesId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignaturaEstudiantes_AsignaturasId",
                table: "AsignaturaEstudiantes",
                column: "AsignaturasId");

            migrationBuilder.CreateIndex(
                name: "IX_AsignaturaEstudiantes_EstudiantesId",
                table: "AsignaturaEstudiantes",
                column: "EstudiantesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsignaturaEstudiantes");
        }
    }
}
