using System;
using Notifications.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsAccess
    {
        Task<IList<NotificationModel>> GetAllNotifications();
        Task<IList<NotificationModel>> GetUserNotifications(Guid userId);
    }
}
