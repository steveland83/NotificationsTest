using System;
using System.Collections.Generic;
using System.Text;
using Notifications.Common.Models.Enums;

namespace Notifications.Common.Models
{
    public class EventModel
    {
        public NotificationEventType EventType { get; set; }
        public EventDataModel Data{ get; set; }
        public Guid UserId { get; set; }
    }
}
