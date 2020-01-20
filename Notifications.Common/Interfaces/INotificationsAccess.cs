using Notifications.Common.Models;
using System.Collections.Generic;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsAccess
    {
        IEnumerable<NotificationModel> GetAllNotifications();
    }
}
