using System;
using Notifications.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsService
    {
        Task<IReadOnlyCollection<NotificationModel>> GetAllNotifications();
        Task<IReadOnlyCollection<NotificationModel>> GetUserNotifications(Guid userId);
        Task<NotificationModel> CreateEventNotification(EventModel eventModel);
    }
}