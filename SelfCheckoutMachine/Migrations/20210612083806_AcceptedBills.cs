using Microsoft.EntityFrameworkCore.Migrations;

namespace SelfCheckoutMachine.Migrations
{
    public partial class AcceptedBills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                });
            migrationBuilder.Sql($"INSERT INTO Bills(BillName) VALUES ('5'), ('10'), ('20'), ('50'), ('100'), ('200'), ('500'), ('1000'), ('2000'), ('5000'), ('10000'), ('20000')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");
        }
    }
}
