using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPP.Migrations
{
    /// <inheritdoc />
    public partial class EditingUserModelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Created_at",
                value: new DateTime(2024, 8, 23, 11, 27, 37, 718, DateTimeKind.Local).AddTicks(6226));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Created_at",
                value: new DateTime(2024, 8, 21, 14, 26, 42, 515, DateTimeKind.Local).AddTicks(4609));
        }
    }
}
