using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Common.Models.Enums;
using Xunit;

namespace Notifications.Services.UnitTests
{
    public class NotificationsServiceTests
    {
        private List<NotificationModel> notificationModels;
        private Mock<INotificationsAccess> mockNotificationsAccess;
        private Mock<INotificationTemplatesAccess> mockNotificationTemplatesAccess;

        public NotificationsServiceTests()
        {
            mockNotificationTemplatesAccess = new Mock<INotificationTemplatesAccess>();
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
            var sut = new NotificationsService(mockNotificationsAccess.Object, mockNotificationTemplatesAccess.Object);

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
            var sut = new NotificationsService(mockNotificationsAccess.Object, mockNotificationTemplatesAccess.Object);

            var result = await sut.GetUserNotifications(userId);

            Assert.All(userNotifications, model => Assert.Contains(model, result));
            Assert.Equal(userNotifications.Count, result.Count);
        }

        [Fact]
        public async Task GetUserNotifications_ReturnsEmptyList_WhenNoMatchesAreFound()
        {
            var userId = Guid.NewGuid();
            mockNotificationsAccess.Setup(x => x.GetUserNotifications(userId)).ReturnsAsync(new List<NotificationModel>());
            var sut = new NotificationsService(mockNotificationsAccess.Object, mockNotificationTemplatesAccess.Object);

            var result = await sut.GetUserNotifications(userId);

            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async Task CreateEventNotification_PopulatesGeneratedNotifications_WithSubstitutedTemplateData()
        {
            var expectedNotificationBody =
                "Hi Nicola, your appointment with Tesla at 01:45 has been - cancelled for the following reason: We signed a deal with Musk.";
            var eventModel = new EventModel
            {
                EventType = NotificationEventType.AppointmentCancelled,
                UserId = Guid.NewGuid(),
                Data = new EventDataModel
                {
                    Firstname = "Nicola",
                    OrganisationName = "Tesla",
                    AppointmentDateTime = new DateTime().AddMinutes(105),
                    Reason = "We signed a deal with Musk"
                }
            };
            var notificationTemplateModel = new NotificationTemplateModel
            {
                Id = Guid.NewGuid(),
                EventType = NotificationEventType.AppointmentCancelled,
                Body = "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                Title = "Appointment Cancelled"
            };

            mockNotificationTemplatesAccess.Setup(x => x.GetNotificationTemplate(eventModel.EventType)).ReturnsAsync(notificationTemplateModel);
            var sut = new NotificationsService(mockNotificationsAccess.Object, mockNotificationTemplatesAccess.Object);
            
            var result = await sut.CreateEventNotification(eventModel);

            Assert.Equal(notificationTemplateModel.Title, result.Title);
            Assert.Equal(notificationTemplateModel.EventType, result.EventType);
            Assert.Equal(eventModel.UserId, result.UserId);
            Assert.Equal(expectedNotificationBody, result.Body);
        }

        [Fact]
        public async Task CreateEventNotification_PersistsNewNotification()
        {
            var expectedNotificationBody =
                "Hi Nicola, your appointment with Tesla at 01:45 has been - cancelled for the following reason: We signed a deal with Musk.";
            var eventModel = new EventModel
            {
                EventType = NotificationEventType.AppointmentCancelled,
                UserId = Guid.NewGuid(),
                Data = new EventDataModel
                {
                    Firstname = "Nicola",
                    OrganisationName = "Tesla",
                    AppointmentDateTime = new DateTime().AddMinutes(105),
                    Reason = "We signed a deal with Musk"
                }
            };
            var notificationTemplateModel = new NotificationTemplateModel
            {
                Id = Guid.NewGuid(),
                EventType = NotificationEventType.AppointmentCancelled,
                Body = "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                Title = "Appointment Cancelled"
            };

            mockNotificationTemplatesAccess.Setup(x => x.GetNotificationTemplate(eventModel.EventType)).ReturnsAsync(notificationTemplateModel);
            var sut = new NotificationsService(mockNotificationsAccess.Object, mockNotificationTemplatesAccess.Object);

            await sut.CreateEventNotification(eventModel);

            mockNotificationsAccess.Verify(x=>x.SaveNotification(It.Is<NotificationModel>(model => 
                model.EventType == eventModel.EventType &&
                model.Title == notificationTemplateModel.Title &&
                model.UserId == eventModel.UserId &&
                model.Body == expectedNotificationBody)));
        }
    }
}
