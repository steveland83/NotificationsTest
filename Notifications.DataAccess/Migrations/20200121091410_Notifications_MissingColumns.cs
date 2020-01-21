using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notifications.DataAccess.Migrations
{
    public partial class Notifications_MissingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "NotificationTemplates",
                "Id",
                new Guid("275349c6-e53c-4409-80fd-5c1bef7f9623"));

            migrationBuilder.AddColumn<string>(
                "Body",
                "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                "EventType",
                "Notifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                "Title",
                "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                "UserId",
                "Notifications",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                "NotificationTemplates",
                new[] {"Id", "Body", "EventType", "Title"},
                new object[]
                {
                    new Guid("318f1444-d60e-41ee-a3ea-f14b12370b81"),
                    "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                    0, "Appointment Cancelled"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "NotificationTemplates",
                "Id",
                new Guid("318f1444-d60e-41ee-a3ea-f14b12370b81"));

            migrationBuilder.DropColumn(
                "Body",
                "Notifications");

            migrationBuilder.DropColumn(
                "EventType",
                "Notifications");

            migrationBuilder.DropColumn(
                "Title",
                "Notifications");

            migrationBuilder.DropColumn(
                "UserId",
                "Notifications");

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
    }
}