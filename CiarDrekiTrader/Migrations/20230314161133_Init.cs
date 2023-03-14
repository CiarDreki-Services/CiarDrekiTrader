using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CiarDrekiTrader.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Val = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnedStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnedStocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Val = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfficialTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Val = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalVal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Generated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Symbol1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PcChange1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Symbol2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PcChange2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Symbol3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PcChange3 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Symbol4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PcChange4 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Symbol5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PcChange5 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopStocks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashTransactions");

            migrationBuilder.DropTable(
                name: "OwnedStocks");

            migrationBuilder.DropTable(
                name: "StockPrices");

            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "TopStocks");
        }
    }
}
