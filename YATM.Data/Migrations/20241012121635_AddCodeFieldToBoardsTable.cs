using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YATM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeFieldToBoardsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Boards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Boards");
        }
    }
}
