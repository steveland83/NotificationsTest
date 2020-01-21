using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notifications.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Notifications",
                table => new
                {
                    Id = table.Column<Guid>()
                },
                constraints: table => { table.PrimaryKey("PK_Notifications", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Notifications");
        }
    }
}