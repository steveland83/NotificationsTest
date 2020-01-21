using System;
using Notifications.Common.Models.Enums;

namespace Notifications.Common.Exceptions
{
    public class EventTypeNotSupportedException : Exception
    {
        public EventTypeNotSupportedException(NotificationEventType eventType) : base(
            $"No matching EventTemplate was found for EventType '{eventType}'")
        {
        }
    }
}