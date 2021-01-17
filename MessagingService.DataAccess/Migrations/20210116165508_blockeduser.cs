using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MessagingService.DataAccess.Migrations
{
    public partial class blockeduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockingUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlockedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlockedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockedUsers");
        }
    }
}
