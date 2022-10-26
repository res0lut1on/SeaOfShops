using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaOfShops.Migrations
{
    public partial class shopname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProduct_Shops_ShopId",
                table: "OrderedProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderedProduct_ShopId",
                table: "OrderedProduct");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "OrderedProduct",
                newName: "ShopName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShopName",
                table: "OrderedProduct",
                newName: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProduct_ShopId",
                table: "OrderedProduct",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProduct_Shops_ShopId",
                table: "OrderedProduct",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "ShopId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
