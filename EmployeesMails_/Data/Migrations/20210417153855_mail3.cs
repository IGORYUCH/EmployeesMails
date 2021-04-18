using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeesMails_.Data.Migrations
{
    public partial class mail3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    From_employeeId = table.Column<int>(nullable: true),
                    To_employeeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mail_Employee_From_employeeId",
                        column: x => x.From_employeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mail_Employee_To_employeeId",
                        column: x => x.To_employeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mail_From_employeeId",
                table: "Mail",
                column: "From_employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mail_To_employeeId",
                table: "Mail",
                column: "To_employeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mail");
        }
    }
}
