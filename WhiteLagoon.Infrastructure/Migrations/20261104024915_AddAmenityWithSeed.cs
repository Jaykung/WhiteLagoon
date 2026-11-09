using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAmenityWithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VillaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amenities_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "Description", "Name", "VillaId" },
                values: new object[,]
                {
                    { 1, "Enjoy your own private pool with stunning views of the surrounding landscape.", "Private Pool", 1 },
                    { 2, "Convenient microwave for quick meals and snacks.", "Microwave", 1 },
                    { 3, "Relax on your private balcony and take in the breathtaking scenery.", "Private Balcony", 1 },
                    { 4, "Spacious bedroom with a comfortable king bed and a sofa bed for additional guests.", "1 king bed and 1 sofa bed", 1 },
                    { 5, "Take a refreshing dip in your own private plunge pool, perfect for relaxation.", "Private Plunge Pool", 2 },
                    { 6, "Convenient microwave and mini refrigerator for storing and preparing your favorite snacks and beverages.", "Microwave and Mini Refrigerator", 2 },
                    { 7, "Enjoy the fresh air and beautiful views from your own private balcony.", "Private Balcony", 2 },
                    { 8, "Choose from a comfortable king bed or two double beds for a good night's sleep.", "king bed or 2 double beds", 2 },
                    { 9, "Enjoy your own private pool with stunning views of the surrounding landscape.", "Private Pool", 3 },
                    { 10, "Relax and unwind in your own private jacuzzi, perfect for a romantic evening or a soothing soak after a long day.", "Jacuzzi", 3 },
                    { 11, "Relax on your private balcony and take in the breathtaking scenery.", "Private Balcony", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_VillaId",
                table: "Amenities",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenities");
        }
    }
}
