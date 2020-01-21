namespace Notifications.Common.Settings
{
    public class NotificationDbConfig
    {
        public string NotificationsDbConnection { get; set; }
        public bool UseInMemoryTestingDb { get; set; } = false;
    }
}