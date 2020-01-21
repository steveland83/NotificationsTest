using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.DataAccess.Access
{
    public class NotificationsAccess : INotificationsAccess
    {
        private readonly NotificationsDbContext dbContext;
        private readonly IMapper mapper;

        public NotificationsAccess(NotificationsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IList<NotificationModel>> GetAllNotifications()
        {
            return await dbContext.Notifications.ProjectTo<NotificationModel>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IList<NotificationModel>> GetUserNotifications(Guid userId)
        {
            return await dbContext.Notifications.ProjectTo<NotificationModel>(mapper.ConfigurationProvider)
                .Where(notification => notification.UserId == userId).ToListAsync();
        }
    }
}