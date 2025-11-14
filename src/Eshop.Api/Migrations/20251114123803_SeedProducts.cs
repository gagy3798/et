using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImgUri", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Hlasový asistent Siri – v angličtine, kompatibilná aplikácia Apple Home, podpora iOS, pripojenie cez WiFi 2,4 GHz a bluetooth, otvorený systém, fungovanie samostatne, ovládanie domácnosti, kamera, 2 mikrofóny na snímanie okolitého zvuku, podporuje Apple Music, basový reproduktor", "https://image.alza.cz/products/JA041a1/JA041a1.jpg?width=500&height=500", "Apple HomePod mini biely", 125.9m },
                    { 2, "Hlasový asistent Amazon Alexa – kompatibilný s aplikáciami výrobcu, podpora Android a iOS, pripojenie cez WiFi 2,4 GHz, otvorený systém, fungovanie samostatne, ovládanie domácnosti, tvorba scenárov, displej, hodiny a tlačidlo na odpojenie mikrofónu, dotykové ovládanie", "https://image.alza.cz/products/AME1047/AME1047.jpg?width=500&height=500", "Amazon Echo Spot Glacier White", 83.9m },
                    { 3, "Hlasový asistent Google Assistant – v angličtine, kompatibilná aplikácia google Home, podpora Android a iOS, pripojenie cez WiFi 2,4 GHz a bluetooth, otvorený systém, fungovanie samostatne, ovládanie domácnosti, 3 mikrofóny na snímanie okolitého zvuku, podporuje Spotify, basový a výškový reproduktor", "https://image.alza.cz/products/GOOGnestA1/GOOGnestA1.jpg?width=500&height=500", "Google Nest Audio Chalk", 92.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
