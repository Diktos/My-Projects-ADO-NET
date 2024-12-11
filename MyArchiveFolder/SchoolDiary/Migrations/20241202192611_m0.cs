using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolDiary.Migrations
{
    /// <inheritdoc />
    public partial class m0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassTeacherId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Teachers_ClassTeacherId",
                        column: x => x.ClassTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsPresent = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "11-А" },
                    { 2, "10-Б" },
                    { 3, "9-Б" },
                    { 4, "8-Б" },
                    { 5, "7-A" },
                    { 6, "6-A" }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "StudentId", "SubjectId", "Value" },
                values: new object[,]
                {
                    { 1, 1, 5, 12 },
                    { 2, 2, 4, 10 },
                    { 3, 1, 1, 11 },
                    { 4, 2, 6, 1 },
                    { 5, 3, 2, 8 },
                    { 6, 4, 6, 7 },
                    { 7, 1, 4, 2 },
                    { 8, 3, 5, 12 },
                    { 9, 2, 1, 8 },
                    { 10, 4, 3, 12 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Артем Стецько" },
                    { 2, "Софія Калина" },
                    { 3, "Валентин Нотатка" },
                    { 4, "Олег Кернасто" },
                    { 5, "Владислав Іскра" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ClassId", "ClassTeacherId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, 4, 1, "Данило", "Міхневич" },
                    { 2, 4, 1, "Стас", "Мікроволновка" },
                    { 3, 1, 2, "Антон", "Потік" },
                    { 4, 2, 3, "Марина", "Сталкер" },
                    { 5, 1, 4, "Богдан", "Компот" },
                    { 6, 6, 2, "Софія", "Роментік" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name", "TeacherId" },
                values: new object[,]
                {
                    { 1, "Математика", 1 },
                    { 2, "Історія України", 2 },
                    { 3, "Англійська мова", 3 },
                    { 4, "Біологія", 4 },
                    { 5, "Українська мова", 5 },
                    { 6, "Сhemistry", 2 }
                });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "AttendanceDate", "IsPresent", "StudentId", "SubjectId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 5 },
                    { 2, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 3 },
                    { 3, new DateTime(2024, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 4 },
                    { 4, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 6, 6 },
                    { 5, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, 2 },
                    { 6, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SubjectId",
                table: "Attendances",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassTeacherId",
                table: "Students",
                column: "ClassTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
