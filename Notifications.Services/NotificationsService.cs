using System;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsAccess notificationsAccess;

        public NotificationsService(INotificationsAccess notificationsAccess)
        {
            this.notificationsAccess = notificationsAccess;
        }

        public async Task<IReadOnlyCollection<NotificationModel>> GetAllNotifications()
        {
            var notifications = await this.notificationsAccess.GetAllNotifications();

            return new ReadOnlyCollection<NotificationModel>(notifications);
        }

        public async Task<IReadOnlyCollection<NotificationModel>> GetUserNotifications(Guid userId)
        {
            var notifications = await this.notificationsAccess.GetUserNotifications(userId);

            return new ReadOnlyCollection<NotificationModel>(notifications);
        }
    }
}
