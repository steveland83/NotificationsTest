using System;

namespace Notifications.Common.Models
{
    public class EventDataModel
    {
        public string Firstname { get; set; }
        public string OrganisationName { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Reason { get; set; }
    }
}