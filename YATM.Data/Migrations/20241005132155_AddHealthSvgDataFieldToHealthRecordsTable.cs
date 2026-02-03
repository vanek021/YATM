using Microsoft.EntityFrameworkCore.Migrations;
using YATM.Models.Entities.Health;

#nullable disable

namespace YATM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthSvgDataFieldToHealthRecordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<HealthSvgData>(
                name: "HealthSvgData",
                table: "HealthRecords",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HealthSvgData",
                table: "HealthRecords");
        }
    }
}
