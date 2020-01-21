using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notifications.DataAccess.Migrations
{
    public partial class Notifications_MissingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("275349c6-e53c-4409-80fd-5c1bef7f9623"));

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "Notifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Notifications",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "NotificationTemplates",
                columns: new[] { "Id", "Body", "EventType", "Title" },
                values: new object[] { new Guid("318f1444-d60e-41ee-a3ea-f14b12370b81"), "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.", 0, "Appointment Cancelled" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "Id",
                keyValue: new Guid("318f1444-d60e-41ee-a3ea-f14b12370b81"));

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.InsertData(
                table: "NotificationTemplates",
                columns: new[] { "Id", "Body", "EventType", "Title" },
                values: new object[] { new Guid("275349c6-e53c-4409-80fd-5c1bef7f9623"), "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.", 0, "Appointment Cancelled" });
        }
    }
}
