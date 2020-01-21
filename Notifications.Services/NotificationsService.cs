using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Notifications.Common.Exceptions;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services.ExtensionMethods;

namespace Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsAccess notificationsAccess;
        private readonly INotificationTemplatesAccess notificationTemplatesAccess;

        public NotificationsService(INotificationsAccess notificationsAccess,
            INotificationTemplatesAccess notificationTemplatesAccess)
        {
            this.notificationsAccess = notificationsAccess;
            this.notificationTemplatesAccess = notificationTemplatesAccess;
        }

        public async Task<IReadOnlyCollection<NotificationModel>> GetAllNotifications()
        {
            var notifications = await notificationsAccess.GetAllNotifications();

            return new ReadOnlyCollection<NotificationModel>(notifications);
        }

        public async Task<IReadOnlyCollection<NotificationModel>> GetUserNotifications(Guid userId)
        {
            var notifications = await notificationsAccess.GetUserNotifications(userId);

            return new ReadOnlyCollection<NotificationModel>(notifications);
        }

        public async Task<NotificationModel> CreateEventNotification(EventModel eventModel)
        {
            var template =
                await notificationTemplatesAccess.GetNotificationTemplate(eventModel.EventType);

            if (template == null) throw new EventTypeNotSupportedException(eventModel.EventType);

            var notification = new NotificationModel
            {
                EventType = template.EventType,
                Title = template.Title,
                UserId = eventModel.UserId,

                // Please note - I'm inferring that the AppointmentDateTime should be time only from the language in the template.
                // In reality I'd query this as it seems likely the date should be included.
                Body =
                    template.Body
                        .ReplaceTemplateField(nameof(EventDataModel.Firstname), eventModel.Data.Firstname)
                        .ReplaceTemplateField(nameof(EventDataModel.OrganisationName), eventModel.Data.OrganisationName)
                        .ReplaceTemplateField(nameof(EventDataModel.AppointmentDateTime),
                            eventModel.Data.AppointmentDateTime.ToShortTimeString())
                        .ReplaceTemplateField(nameof(EventDataModel.Reason), eventModel.Data.Reason)
            };

            return await notificationsAccess.SaveNotification(notification);
        }
    }
}