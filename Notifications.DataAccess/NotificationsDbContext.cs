using System;
using Microsoft.EntityFrameworkCore;
using Notifications.Common.Models.Enums;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {

        }

        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<NotificationTemplateEntity> NotificationTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NotificationTemplateEntity>().HasData(new NotificationTemplateEntity()
            {
                Id = Guid.NewGuid(),
                EventType = NotificationEventType.AppointmentCancelled,
                Body = "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                Title = "Appointment Cancelled"
            });
        }
    }
}
