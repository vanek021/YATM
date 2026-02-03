using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YATM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerFieldToNoteTagsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "NoteTags",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteTags_OwnerId",
                table: "NoteTags",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTags_Users_OwnerId",
                table: "NoteTags",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTags_Users_OwnerId",
                table: "NoteTags");

            migrationBuilder.DropIndex(
                name: "IX_NoteTags_OwnerId",
                table: "NoteTags");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "NoteTags");
        }
    }
}
