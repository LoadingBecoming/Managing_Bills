using Microsoft.EntityFrameworkCore.Migrations;

namespace TestWebAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    ProductDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductPropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    ShellPrice = table.Column<double>(type: "float", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.ProductDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertySort = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.PropertyId);
                    table.ForeignKey(
                        name: "FK_Properties_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyDetails",
                columns: table => new
                {
                    PropertyDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    PropertiesPropertyId = table.Column<int>(type: "int", nullable: true),
                    PropertyDetailCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyDetailDetail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDetails", x => x.PropertyDetailId);
                    table.ForeignKey(
                        name: "FK_PropertyDetails_Properties_PropertiesPropertyId",
                        column: x => x.PropertiesPropertyId,
                        principalTable: "Properties",
                        principalColumn: "PropertyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetailPropertyDetails",
                columns: table => new
                {
                    ProductDetailPropertyDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDetailId = table.Column<int>(type: "int", nullable: false),
                    ProductDetailsProductDetailId = table.Column<int>(type: "int", nullable: true),
                    PropertyDetailId = table.Column<int>(type: "int", nullable: false),
                    PropertyDetailsPropertyDetailId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetailPropertyDetails", x => x.ProductDetailPropertyDetailId);
                    table.ForeignKey(
                        name: "FK_ProductDetailPropertyDetails_ProductDetails_ProductDetailsProductDetailId",
                        column: x => x.ProductDetailsProductDetailId,
                        principalTable: "ProductDetails",
                        principalColumn: "ProductDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductDetailPropertyDetails_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductDetailPropertyDetails_PropertyDetails_PropertyDetailsPropertyDetailId",
                        column: x => x.PropertyDetailsPropertyDetailId,
                        principalTable: "PropertyDetails",
                        principalColumn: "PropertyDetailId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailPropertyDetails_ProductDetailsProductDetailId",
                table: "ProductDetailPropertyDetails",
                column: "ProductDetailsProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailPropertyDetails_ProductsProductId",
                table: "ProductDetailPropertyDetails",
                column: "ProductsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetailPropertyDetails_PropertyDetailsPropertyDetailId",
                table: "ProductDetailPropertyDetails",
                column: "PropertyDetailsPropertyDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ProductsProductId",
                table: "Properties",
                column: "ProductsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDetails_PropertiesPropertyId",
                table: "PropertyDetails",
                column: "PropertiesPropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDetailPropertyDetails");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "PropertyDetails");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
