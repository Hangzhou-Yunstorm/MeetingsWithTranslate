using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Yunstorm.Meeting.Migrations
{
    public partial class AddMeetingRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeetingRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    SessionCode = table.Column<string>(maxLength: 10, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sender = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(maxLength: 1024, nullable: true),
                    Language = table.Column<string>(nullable: false),
                    SendingTime = table.Column<DateTime>(nullable: false),
                    IsVoice = table.Column<bool>(nullable: false, defaultValue: false),
                    VoiceFile = table.Column<string>(maxLength: 512, nullable: true),
                    MeetingRoomId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_MeetingRooms_MeetingRoomId",
                        column: x => x.MeetingRoomId,
                        principalTable: "MeetingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniqueId = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    JoinTime = table.Column<DateTime>(nullable: false),
                    MeetingRoomId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participant_MeetingRooms_MeetingRoomId",
                        column: x => x.MeetingRoomId,
                        principalTable: "MeetingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MeetingRoomId",
                table: "Messages",
                column: "MeetingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_MeetingRoomId",
                table: "Participant",
                column: "MeetingRoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "MeetingRooms");
        }
    }
}
