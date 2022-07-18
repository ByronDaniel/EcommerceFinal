using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BP.Ecommerce.Infraestructure.Migrations
{
    public partial class addColumnTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "OrderProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "OrderProducts");
        }
    }
}
