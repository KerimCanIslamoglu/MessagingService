using Microsoft.EntityFrameworkCore.Migrations;

namespace MessagingService.DataAccess.Migrations
{
    public partial class messagesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendedAt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
