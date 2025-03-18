using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAppointment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addprofessionalunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SLMC",
                table: "Professionals",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_SLMC",
                table: "Professionals",
                column: "SLMC",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Professionals_SLMC",
                table: "Professionals");

            migrationBuilder.AlterColumn<string>(
                name: "SLMC",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
