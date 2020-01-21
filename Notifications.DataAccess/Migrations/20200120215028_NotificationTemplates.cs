using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notifications.DataAccess.Migrations
{
    public partial class NotificationTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "NotificationTemplates",
                table => new
                {
                    Id = table.Column<Guid>(),
                    EventType = table.Column<int>(),
                    Body = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_NotificationTemplates", x => x.Id); });

            migrationBuilder.InsertData(
                "NotificationTemplates",
                new[] {"Id", "Body", "EventType", "Title"},
                new object[]
                {
                    new Guid("275349c6-e53c-4409-80fd-5c1bef7f9623"),
                    "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                    0, "Appointment Cancelled"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "NotificationTemplates");
        }
    }
}