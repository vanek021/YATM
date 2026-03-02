using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YATM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update__AddRecipeSourceTableAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SourceSiteId",
                table: "Recipes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceUrl",
                table: "Recipes",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecipeSourceSites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSourceSites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SourceSiteId",
                table: "Recipes",
                column: "SourceSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceSites_Code",
                table: "RecipeSourceSites",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceSites_Id",
                table: "RecipeSourceSites",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeSourceSites_SourceSiteId",
                table: "Recipes",
                column: "SourceSiteId",
                principalTable: "RecipeSourceSites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeSourceSites_SourceSiteId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "RecipeSourceSites");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SourceSiteId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourceSiteId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourceUrl",
                table: "Recipes");
        }
    }
}
