using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organizer3.Migrations
{
    public partial class m30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddRecruitmentNoteModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aplicant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteContent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddRecruitmentNoteModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilitiesListModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitiesListModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recruitmentNotes_RecruitmentId",
                table: "recruitmentNotes",
                column: "RecruitmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_recruitmentNotes_Recruitments_RecruitmentId",
                table: "recruitmentNotes",
                column: "RecruitmentId",
                principalTable: "Recruitments",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recruitmentNotes_Recruitments_RecruitmentId",
                table: "recruitmentNotes");

            migrationBuilder.DropTable(
                name: "AddRecruitmentNoteModel");

            migrationBuilder.DropTable(
                name: "FacilitiesListModel");

            migrationBuilder.DropIndex(
                name: "IX_recruitmentNotes_RecruitmentId",
                table: "recruitmentNotes");
        }
    }
}
