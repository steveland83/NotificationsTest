using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Notifications.DataAccess.Access
{
    public class NotificationsAccess : INotificationsAccess
    {
        private readonly NotificationsDbContext dbContext;

        public NotificationsAccess(NotificationsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<NotificationModel> GetAllNotifications()
        {
            return dbContext.Notifications.Select(x => new NotificationModel()
            {
                Id = x.Id,
            });
        }
    }
}
