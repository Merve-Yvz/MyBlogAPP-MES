using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogAPP.Migrations
{
    /// <inheritdoc />
    public partial class FixedLikeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Created_at",
                value: new DateTime(2024, 9, 2, 17, 44, 54, 259, DateTimeKind.Local).AddTicks(2148));

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CommentID",
                table: "Likes",
                column: "CommentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Likes_CommentID",
                table: "Likes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "Created_at",
                value: new DateTime(2024, 9, 2, 17, 13, 57, 52, DateTimeKind.Local).AddTicks(2786));
        }
    }
}
