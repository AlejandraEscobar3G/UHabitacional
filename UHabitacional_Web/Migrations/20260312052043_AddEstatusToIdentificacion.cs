using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UHabitacional_Web.Migrations
{
    /// <inheritdoc />
    public partial class AddEstatusToIdentificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estatus",
                table: "TiposUsuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Estatus",
                table: "Identificacion",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Identificacion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Identificacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Identificacion",
                keyColumn: "Id",
                keyValue: 3,
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Estatus",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "Estatus",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "Estatus",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "TiposUsuario");

            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "Identificacion");
        }
    }
}
