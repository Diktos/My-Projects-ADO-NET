using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cafe.Migrations
{
    /// <inheritdoc />
    public partial class m0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nomenclatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nomenclatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    Bill = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_ClientTables_ClientTableId",
                        column: x => x.ClientTableId,
                        principalTable: "ClientTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WaiterId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_WaiterId",
                        column: x => x.WaiterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomenclatureId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Sum = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Nomenclatures_NomenclatureId",
                        column: x => x.NomenclatureId,
                        principalTable: "Nomenclatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClientTables",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Столик біля вікна" },
                    { 2, "Столик 1" },
                    { 3, "Столик 2" }
                });

            migrationBuilder.InsertData(
                table: "Nomenclatures",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Суп гороховий", 45.0 },
                    { 2, "Суп сирний", 75.0 },
                    { 3, "Суп харчо", 55.0 },
                    { 4, "Лоці", 105.0 },
                    { 5, "Картопля фрі", 60.0 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Manager" },
                    { 3, "Waiter" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Name", "Password" },
                values: new object[,]
                {
                    { 1, null, "Admin", "12345" },
                    { 2, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Waiter", "12345" },
                    { 3, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", "12345" },
                    { 4, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Barmen", "12345" },
                    { 5, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cook", "12345" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Bill", "ClientTableId", "Date", "UserId" },
                values: new object[,]
                {
                    { 1, 0.0, 2, new DateTime(2024, 3, 2, 9, 30, 23, 0, DateTimeKind.Unspecified), 2 },
                    { 2, 0.0, 3, new DateTime(2024, 3, 2, 9, 50, 23, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 0.0, 1, new DateTime(2024, 3, 2, 9, 5, 23, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "WaiterId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "Count", "NomenclatureId", "OrderId", "Price", "Sum" },
                values: new object[,]
                {
                    { 1, 2.0, 1, 1, 45.0, 90.0 },
                    { 2, 2.0, 4, 1, 105.0, 210.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_NomenclatureId",
                table: "OrderDetails",
                column: "NomenclatureId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientTableId",
                table: "Orders",
                column: "ClientTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_WaiterId",
                table: "UserRoles",
                column: "WaiterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Nomenclatures");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ClientTables");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
