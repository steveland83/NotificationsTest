using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess.Access
{
    public class NotificationsAccess : INotificationsAccess
    {
        private readonly NotificationsDbContext dbContext;
        private readonly IMapper mapper;

        public NotificationsAccess(NotificationsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IList<NotificationModel>> GetAllNotifications()
        {
            var results = await dbContext.Notifications.ToListAsync();

            return results.Select(x => mapper.Map<NotificationModel>(x)).ToList();
        }

        public async Task<IList<NotificationModel>> GetUserNotifications(Guid userId)
        {
            var results = await dbContext.Notifications.ProjectTo<NotificationModel>(mapper.ConfigurationProvider)
                .Where(notification => notification.UserId == userId).ToListAsync();

            return results.Select(x => mapper.Map<NotificationModel>(x)).ToList();
        }

        public async Task<NotificationModel> SaveNotification(NotificationModel notificationModel)
        {
            var notificationEntity = mapper.Map<NotificationEntity>(notificationModel);

            if (notificationModel.Id == Guid.Empty)
                dbContext.Notifications.Add(notificationEntity);
            else
                dbContext.Notifications.Update(notificationEntity);

            await dbContext.SaveChangesAsync();

            return mapper.Map<NotificationModel>(notificationEntity);
        }
    }
}