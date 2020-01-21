using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notifications.Common.Models.Enums;

namespace Notifications.DataAccess.Entities
{
    public class NotificationEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public NotificationEventType EventType { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
    }
}
