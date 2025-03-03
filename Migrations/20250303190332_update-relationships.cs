using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeAPI.Migrations
{
    /// <inheritdoc />
    public partial class updaterelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Persons",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Jobs",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Educations",
                newName: "ID");

            migrationBuilder.AddColumn<int>(
                name: "PersonID_FK",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonID_FK",
                table: "Educations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PersonID_FK",
                table: "Jobs",
                column: "PersonID_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_PersonID_FK",
                table: "Educations",
                column: "PersonID_FK");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Persons_PersonID_FK",
                table: "Educations",
                column: "PersonID_FK",
                principalTable: "Persons",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Persons_PersonID_FK",
                table: "Jobs",
                column: "PersonID_FK",
                principalTable: "Persons",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Persons_PersonID_FK",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Persons_PersonID_FK",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_PersonID_FK",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Educations_PersonID_FK",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "PersonID_FK",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PersonID_FK",
                table: "Educations");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Persons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Jobs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Educations",
                newName: "Id");
        }
    }
}
