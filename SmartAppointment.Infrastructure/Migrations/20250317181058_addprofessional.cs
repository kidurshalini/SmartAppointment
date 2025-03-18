using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartAppointment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addprofessional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Professionals",
                newName: "SLMC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SLMC",
                table: "Professionals",
                newName: "UserId");
        }
    }
}
