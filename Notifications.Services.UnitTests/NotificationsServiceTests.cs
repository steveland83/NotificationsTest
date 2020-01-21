using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Xunit;

namespace Notifications.Services.UnitTests
{
    public class NotificationsServiceTests
    {
        private List<NotificationModel> notificationModels;
        private Mock<INotificationsAccess> mockNotificationsAccess;

        public NotificationsServiceTests()
        {
            mockNotificationsAccess = new Mock<INotificationsAccess>();
            notificationModels = new List<NotificationModel>
            {
                new NotificationModel()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                },
                new NotificationModel()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                },
                new NotificationModel()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                },
                new NotificationModel()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };
        }

        [Fact]
        public async Task GetAllNotifications_ReturnsAllNotifications()
        {
            mockNotificationsAccess.Setup(x => x.GetAllNotifications()).ReturnsAsync(notificationModels);
            var sut = new NotificationsService(mockNotificationsAccess.Object);

            var result = await sut.GetAllNotifications();

            Assert.All(notificationModels, model => Assert.Contains(model, result));
        }

        [Fact]
        public async Task GetUserNotifications_ReturnsAllUserNotifications_ForThatUserOnly()
        {
            var userId = Guid.NewGuid();
            var userNotifications = new List<NotificationModel>()
            {
                new NotificationModel
                {
                    UserId = userId,
                    Id = Guid.NewGuid()
                },
                new NotificationModel
                {
                    UserId = userId,
                    Id = Guid.NewGuid()
                }
            };
            notificationModels.AddRange(userNotifications);
            mockNotificationsAccess.Setup(x => x.GetUserNotifications(userId)).ReturnsAsync(userNotifications);
            var sut = new NotificationsService(mockNotificationsAccess.Object);

            var result = await sut.GetUserNotifications(userId);

            Assert.All(userNotifications, model => Assert.Contains(model, result));
            Assert.Equal(userNotifications.Count, result.Count);
        }

        [Fact]
        public async Task GetUserNotifications_ReturnsEmptyList_WhenNoMatchesAreFound()
        {
            var userId = Guid.NewGuid();
            mockNotificationsAccess.Setup(x => x.GetUserNotifications(userId)).ReturnsAsync(new List<NotificationModel>());
            var sut = new NotificationsService(mockNotificationsAccess.Object);

            var result = await sut.GetUserNotifications(userId);

            Assert.Equal(0, result.Count);
        }
    }
}
