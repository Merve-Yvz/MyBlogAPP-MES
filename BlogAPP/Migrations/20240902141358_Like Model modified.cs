using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPP.Migrations
{
    /// <inheritdoc />
    public partial class LikeModelmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Likes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Created_at",
                value: new DateTime(2024, 9, 2, 17, 13, 57, 52, DateTimeKind.Local).AddTicks(2786));

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserID",
                table: "Likes",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserID",
                table: "Likes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

     
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserID",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserID",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Likes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Created_at",
                value: new DateTime(2024, 8, 23, 11, 27, 37, 718, DateTimeKind.Local).AddTicks(6226));
        }
    }
}
