using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_practical.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVariantFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Operand2",
                table: "Variants",
                newName: "Value2");

            migrationBuilder.RenameColumn(
                name: "Operand1",
                table: "Variants",
                newName: "Value1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Variants",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Variants");

            migrationBuilder.RenameColumn(
                name: "Value2",
                table: "Variants",
                newName: "Operand2");

            migrationBuilder.RenameColumn(
                name: "Value1",
                table: "Variants",
                newName: "Operand1");
        }
    }
}
