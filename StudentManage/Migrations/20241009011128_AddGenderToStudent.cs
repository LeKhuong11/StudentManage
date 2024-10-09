using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManage.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "S_Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "S_Students");
        }
    }
}
