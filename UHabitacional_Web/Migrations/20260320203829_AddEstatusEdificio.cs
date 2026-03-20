using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UHabitacional_Web.Migrations
{
    /// <inheritdoc />
    public partial class AddEstatusEdificio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estatus",
                table: "Edificio",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Edificio",
                keyColumn: "Id",
                keyValue: "1-1",
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Edificio",
                keyColumn: "Id",
                keyValue: "1-2",
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Edificio",
                keyColumn: "Id",
                keyValue: "1-3",
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Edificio",
                keyColumn: "Id",
                keyValue: "1-4",
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Edificio",
                keyColumn: "Id",
                keyValue: "1-5",
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "Estatus",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TiposUsuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "Estatus",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "Edificio");

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
    }
}
