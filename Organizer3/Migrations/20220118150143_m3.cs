using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organizer3.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "EmploymentStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentStatuses_FacilityId",
                table: "EmploymentStatuses",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentStatuses_Facility_FacilityId",
                table: "EmploymentStatuses",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentStatuses_Facility_FacilityId",
                table: "EmploymentStatuses");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropIndex(
                name: "IX_EmploymentStatuses_FacilityId",
                table: "EmploymentStatuses");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "EmploymentStatuses");
        }
    }
}
