using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacionWebProgramacion3.Migrations
{
    /// <inheritdoc />
    public partial class Suelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fertilizantes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    forma = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    composicion = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    tipo = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    descripcion = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Fertiliz__3213E83F2C30A0AA", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Plantas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    nombre_cientifico = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    nombre_vulgar = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    autor = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    epoca_floracion = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    altura_maxima = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Plantas__3213E83FB3CA9534", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Suelos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pH = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suelos", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fertilizantes");

            migrationBuilder.DropTable(
                name: "Plantas");

            migrationBuilder.DropTable(
                name: "Suelos");
        }
    }
}
