﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YATM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPinnedToNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "Notes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "Notes");
        }
    }
}
