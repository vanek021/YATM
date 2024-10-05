using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YATM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesFieldsToHealthRecordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BodyNote",
                table: "HealthRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemperatureGeneralNote",
                table: "HealthRecords",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyNote",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "TemperatureGeneralNote",
                table: "HealthRecords");
        }
    }
}
