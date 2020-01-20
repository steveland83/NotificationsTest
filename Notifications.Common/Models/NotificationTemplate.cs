using System;
using System.Collections.Generic;
using System.Text;
using Notifications.Common.Models.Enums;

namespace Notifications.Common.Models
{
    public class NotificationTemplate
    {
        public Guid Id { get; set; }
        public NotificationEventType EventType { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
    }
}
