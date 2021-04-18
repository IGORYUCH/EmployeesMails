using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeesMails_.Data.Migrations
{
    public partial class schemad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mail_Employee_From_employeeId",
                table: "Mail");

            migrationBuilder.DropIndex(
                name: "IX_Mail_From_employeeId",
                table: "Mail");

            migrationBuilder.DropColumn(
                name: "From_employeeId",
                table: "Mail");

            migrationBuilder.AddColumn<int>(
                name: "awdawdawdawdawdId",
                table: "Mail",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mail_awdawdawdawdawdId",
                table: "Mail",
                column: "awdawdawdawdawdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mail_Employee_awdawdawdawdawdId",
                table: "Mail",
                column: "awdawdawdawdawdId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mail_Employee_awdawdawdawdawdId",
                table: "Mail");

            migrationBuilder.DropIndex(
                name: "IX_Mail_awdawdawdawdawdId",
                table: "Mail");

            migrationBuilder.DropColumn(
                name: "awdawdawdawdawdId",
                table: "Mail");

            migrationBuilder.AddColumn<int>(
                name: "From_employeeId",
                table: "Mail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mail_From_employeeId",
                table: "Mail",
                column: "From_employeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mail_Employee_From_employeeId",
                table: "Mail",
                column: "From_employeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
