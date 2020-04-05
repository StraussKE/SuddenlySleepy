using Microsoft.EntityFrameworkCore.Migrations;

namespace SuddenlySleepy.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SSUserSSEvent_SSEvents_SSEventId",
                table: "SSUserSSEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_SSUserSSEvent_AspNetUsers_SSUserId",
                table: "SSUserSSEvent");

            migrationBuilder.RenameColumn(
                name: "SSEventId",
                table: "SSUserSSEvent",
                newName: "sSEventId");

            migrationBuilder.RenameColumn(
                name: "SSUserId",
                table: "SSUserSSEvent",
                newName: "sSUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SSUserSSEvent_SSEventId",
                table: "SSUserSSEvent",
                newName: "IX_SSUserSSEvent_sSEventId");

            migrationBuilder.AlterColumn<string>(
                name: "SSEventName",
                table: "SSEvents",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "SSEvents",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SSEvents",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SSUserSSEvent_SSEvents_sSEventId",
                table: "SSUserSSEvent",
                column: "sSEventId",
                principalTable: "SSEvents",
                principalColumn: "SSEventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SSUserSSEvent_AspNetUsers_sSUserId",
                table: "SSUserSSEvent",
                column: "sSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SSUserSSEvent_SSEvents_sSEventId",
                table: "SSUserSSEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_SSUserSSEvent_AspNetUsers_sSUserId",
                table: "SSUserSSEvent");

            migrationBuilder.RenameColumn(
                name: "sSEventId",
                table: "SSUserSSEvent",
                newName: "SSEventId");

            migrationBuilder.RenameColumn(
                name: "sSUserId",
                table: "SSUserSSEvent",
                newName: "SSUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SSUserSSEvent_sSEventId",
                table: "SSUserSSEvent",
                newName: "IX_SSUserSSEvent_SSEventId");

            migrationBuilder.AlterColumn<string>(
                name: "SSEventName",
                table: "SSEvents",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 75);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "SSEvents",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SSEvents",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 5000);

            migrationBuilder.AddForeignKey(
                name: "FK_SSUserSSEvent_SSEvents_SSEventId",
                table: "SSUserSSEvent",
                column: "SSEventId",
                principalTable: "SSEvents",
                principalColumn: "SSEventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SSUserSSEvent_AspNetUsers_SSUserId",
                table: "SSUserSSEvent",
                column: "SSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
