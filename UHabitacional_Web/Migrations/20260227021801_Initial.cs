using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UHabitacional_Web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Edificio",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TotalDeptos = table.Column<int>(type: "int", nullable: false),
                    NumeroPisos = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edificio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroInt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Piso = table.Column<int>(type: "int", nullable: false),
                    EdificioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departamento_Edificio_EdificioId",
                        column: x => x.EdificioId,
                        principalTable: "Edificio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Estatus = table.Column<int>(type: "int", nullable: false),
                    TipoUsuarioId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_TiposUsuario_TipoUsuarioId",
                        column: x => x.TipoUsuarioId,
                        principalTable: "TiposUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraVisitante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Estatus = table.Column<int>(type: "int", nullable: false),
                    FechaHoraLlegada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaHoraSalida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    codigoVisita = table.Column<int>(type: "int", nullable: false),
                    IdentificacionId = table.Column<int>(type: "int", nullable: false),
                    DepartamentoId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraVisitante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BitacoraVisitante_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BitacoraVisitante_Identificacion_IdentificacionId",
                        column: x => x.IdentificacionId,
                        principalTable: "Identificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BitacoraVigilante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitacoraVigilante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BitacoraVigilante_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inquilino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DepartamentoId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModifyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquilino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inquilino_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inquilino_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Edificio",
                columns: new[] { "Id", "Calle", "ModifyAt", "ModifyBy", "NumeroPisos", "TotalDeptos" },
                values: new object[,]
                {
                    { "1-1", "Av. Chimalhuacán", null, null, 6, 12 },
                    { "1-2", "Calle Pantitlán", null, null, 4, 8 },
                    { "1-3", "Av. Bordo de Xochiaca", null, null, 7, 15 },
                    { "1-4", "Calle Sor Juana Inés de la Cruz", null, null, 5, 10 },
                    { "1-5", "Av. Adolfo López Mateos", null, null, 10, 20 }
                });

            migrationBuilder.InsertData(
                table: "Identificacion",
                columns: new[] { "Id", "Descripcion", "ModifyAt", "ModifyBy" },
                values: new object[,]
                {
                    { 1, "INE", null, null },
                    { 2, "Pasaporte", null, null },
                    { 3, "Licencia de conducir", null, null }
                });

            migrationBuilder.InsertData(
                table: "TiposUsuario",
                columns: new[] { "Id", "Descripcion", "ModifyAt", "ModifyBy" },
                values: new object[,]
                {
                    { 1, "Administrador", null, null },
                    { 2, "Inquilino", null, null },
                    { 3, "Vigilante", null, null }
                });

            migrationBuilder.InsertData(
                table: "Departamento",
                columns: new[] { "Id", "EdificioId", "ModifyAt", "ModifyBy", "NumeroInt", "Piso" },
                values: new object[,]
                {
                    { 1, "1-1", null, null, "101", 1 },
                    { 2, "1-1", null, null, "102", 1 },
                    { 3, "1-2", null, null, "201", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraVigilante_UsuarioId",
                table: "BitacoraVigilante",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraVisitante_DepartamentoId",
                table: "BitacoraVisitante",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_BitacoraVisitante_IdentificacionId",
                table: "BitacoraVisitante",
                column: "IdentificacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Departamento_EdificioId",
                table: "Departamento",
                column: "EdificioId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquilino_DepartamentoId",
                table: "Inquilino",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inquilino_UsuarioId",
                table: "Inquilino",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TipoUsuarioId",
                table: "Usuarios",
                column: "TipoUsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BitacoraVigilante");

            migrationBuilder.DropTable(
                name: "BitacoraVisitante");

            migrationBuilder.DropTable(
                name: "Inquilino");

            migrationBuilder.DropTable(
                name: "Identificacion");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Edificio");

            migrationBuilder.DropTable(
                name: "TiposUsuario");
        }
    }
}
