using Microsoft.EntityFrameworkCore.Migrations;

namespace MessagingService.DataAccess.Migrations
{
    public partial class userLogs2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityDescription",
                table: "UserLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityDescription",
                table: "UserLogs");
        }
    }
}
