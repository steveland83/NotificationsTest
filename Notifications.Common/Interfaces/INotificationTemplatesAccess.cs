using System.Threading.Tasks;
using Notifications.Common.Models;
using Notifications.Common.Models.Enums;

namespace Notifications.Common.Interfaces
{
    public interface INotificationTemplatesAccess
    {
        Task<NotificationTemplateModel> GetNotificationTemplate(NotificationEventType eventType);
    }
}