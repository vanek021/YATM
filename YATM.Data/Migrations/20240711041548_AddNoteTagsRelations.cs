using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YATM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteTagsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoteTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteNoteTags",
                columns: table => new
                {
                    NoteId = table.Column<long>(type: "bigint", nullable: false),
                    NoteTagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteNoteTags", x => new { x.NoteId, x.NoteTagId });
                    table.ForeignKey(
                        name: "FK_NoteNoteTags_NoteTags_NoteTagId",
                        column: x => x.NoteTagId,
                        principalTable: "NoteTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteNoteTags_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteNoteTags_NoteTagId",
                table: "NoteNoteTags",
                column: "NoteTagId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTags_Id",
                table: "NoteTags",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteNoteTags");

            migrationBuilder.DropTable(
                name: "NoteTags");
        }
    }
}
