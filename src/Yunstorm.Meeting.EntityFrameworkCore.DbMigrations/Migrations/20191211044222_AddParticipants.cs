using Microsoft.EntityFrameworkCore.Migrations;

namespace Yunstorm.Meeting.Migrations
{
    public partial class AddParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participant_MeetingRooms_MeetingRoomId",
                table: "Participant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participant",
                table: "Participant");

            migrationBuilder.RenameTable(
                name: "Participant",
                newName: "Participants");

            migrationBuilder.RenameIndex(
                name: "IX_Participant_MeetingRoomId",
                table: "Participants",
                newName: "IX_Participants_MeetingRoomId");

            migrationBuilder.AlterColumn<string>(
                name: "UniqueId",
                table: "Participants",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Participants",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Participants",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Participants",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_MeetingRooms_MeetingRoomId",
                table: "Participants",
                column: "MeetingRoomId",
                principalTable: "MeetingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_MeetingRooms_MeetingRoomId",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Participants");

            migrationBuilder.RenameTable(
                name: "Participants",
                newName: "Participant");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_MeetingRoomId",
                table: "Participant",
                newName: "IX_Participant_MeetingRoomId");

            migrationBuilder.AlterColumn<string>(
                name: "UniqueId",
                table: "Participant",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Participant",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Participant",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participant",
                table: "Participant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_MeetingRooms_MeetingRoomId",
                table: "Participant",
                column: "MeetingRoomId",
                principalTable: "MeetingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
