using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Common.Models.Enums;

namespace Notifications.DataAccess.Access
{
    public class NotificationTemplatesAccess : INotificationTemplatesAccess
    {
        private readonly NotificationsDbContext dbContext;
        private readonly IMapper mapper;

        public NotificationTemplatesAccess(NotificationsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<NotificationTemplateModel> GetNotificationTemplate(NotificationEventType eventType)
        {
            var result = await dbContext.NotificationTemplates.FirstOrDefaultAsync(x => x.EventType == eventType);

            if (result != null)
                return mapper.Map<NotificationTemplateModel>(result);

            return null;
        }
    }
}