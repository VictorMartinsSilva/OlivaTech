using Microsoft.EntityFrameworkCore.Migrations;

namespace OlivaTech.Site.Migrations.ContextDbMigrations
{
    public partial class AddInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CursoTipos",
                columns: table => new
                {
                    CursoTipoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursoTipos", x => x.CursoTipoId);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    CursoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UF = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false),
                    CursoTipoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.CursoId);
                    table.ForeignKey(
                        name: "FK_Cursos_CursoTipos_CursoTipoId",
                        column: x => x.CursoTipoId,
                        principalTable: "CursoTipos",
                        principalColumn: "CursoTipoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    OfertaId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Preco = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false),
                    CursoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.OfertaId);
                    table.ForeignKey(
                        name: "FK_Ofertas_Cursos_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Cursos",
                        principalColumn: "CursoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CursoTipos",
                columns: new[] { "CursoTipoId", "Nome" },
                values: new object[,]
                {
                    { 1L, "Pós-Graduação" },
                    { 2L, "Bacharelado" },
                    { 3L, "Técnico" },
                    { 4L, "Mestrado" }
                });

            migrationBuilder.InsertData(
                table: "Cursos",
                columns: new[] { "CursoId", "Cidade", "CursoTipoId", "Disponivel", "Nome", "UF" },
                values: new object[] { 2L, "Serrana", 1L, true, "Tecnologias para Aplicações Web", "SP" });

            migrationBuilder.InsertData(
                table: "Cursos",
                columns: new[] { "CursoId", "Cidade", "CursoTipoId", "Disponivel", "Nome", "UF" },
                values: new object[] { 1L, "Volta Redonda", 2L, true, "Sistema de Informação", "RJ" });

            migrationBuilder.InsertData(
                table: "Cursos",
                columns: new[] { "CursoId", "Cidade", "CursoTipoId", "Disponivel", "Nome", "UF" },
                values: new object[] { 3L, "Volta Redonda", 3L, false, "Informática", "RJ" });

            migrationBuilder.InsertData(
                table: "Ofertas",
                columns: new[] { "OfertaId", "CursoId", "Disponivel", "Preco" },
                values: new object[] { 2L, 2L, true, 158.58m });

            migrationBuilder.InsertData(
                table: "Ofertas",
                columns: new[] { "OfertaId", "CursoId", "Disponivel", "Preco" },
                values: new object[] { 1L, 1L, true, 306.76m });

            migrationBuilder.InsertData(
                table: "Ofertas",
                columns: new[] { "OfertaId", "CursoId", "Disponivel", "Preco" },
                values: new object[] { 3L, 3L, false, 230.5m });

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_CursoTipoId",
                table: "Cursos",
                column: "CursoTipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_CursoId",
                table: "Ofertas",
                column: "CursoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "CursoTipos");
        }
    }
}
