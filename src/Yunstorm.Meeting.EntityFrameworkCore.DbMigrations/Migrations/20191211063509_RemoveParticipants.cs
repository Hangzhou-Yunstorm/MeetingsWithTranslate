using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Yunstorm.Meeting.Migrations
{
    public partial class RemoveParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Messages",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Messages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConnectionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    JoinTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MeetingRoomId = table.Column<Guid>(type: "uuid", nullable: true),
                    Nickname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UniqueId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_MeetingRooms_MeetingRoomId",
                        column: x => x.MeetingRoomId,
                        principalTable: "MeetingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_MeetingRoomId",
                table: "Participants",
                column: "MeetingRoomId");
        }
    }
}
