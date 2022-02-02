using Microsoft.EntityFrameworkCore.Migrations;

namespace Chondok.Data.Migrations
{
    public partial class AddStockForeignKeyInProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "StockId",
            //    table: "products",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_products_StockId",
            //    table: "products",
            //    column: "StockId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_products_Stocks_StockId",
            //    table: "products",
            //    column: "StockId",
            //    principalTable: "Stocks",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_Stocks_StockId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_StockId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "products");
        }
    }
}
