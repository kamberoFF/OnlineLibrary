using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LastStableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistory_AspNetUsers_UserId",
                table: "ReadingHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistory_AspNetUsers_UserId1",
                table: "ReadingHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistory_Books_BookId",
                table: "ReadingHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistory_Books_BookId1",
                table: "ReadingHistory");

            migrationBuilder.DropIndex(
                name: "IX_ReadingHistory_BookId",
                table: "ReadingHistory");

            migrationBuilder.DropIndex(
                name: "IX_ReadingHistory_BookId1",
                table: "ReadingHistory");

            migrationBuilder.DropIndex(
                name: "IX_ReadingHistory_UserId",
                table: "ReadingHistory");

            migrationBuilder.DropIndex(
                name: "IX_ReadingHistory_UserId1",
                table: "ReadingHistory");

            migrationBuilder.DropColumn(
                name: "BookId1",
                table: "ReadingHistory");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ReadingHistory");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReadingHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BookId",
                table: "ReadingHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReadingHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BookId",
                table: "ReadingHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BookId1",
                table: "ReadingHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ReadingHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_BookId",
                table: "ReadingHistory",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_BookId1",
                table: "ReadingHistory",
                column: "BookId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_UserId",
                table: "ReadingHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_UserId1",
                table: "ReadingHistory",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistory_AspNetUsers_UserId",
                table: "ReadingHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistory_AspNetUsers_UserId1",
                table: "ReadingHistory",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistory_Books_BookId",
                table: "ReadingHistory",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistory_Books_BookId1",
                table: "ReadingHistory",
                column: "BookId1",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
