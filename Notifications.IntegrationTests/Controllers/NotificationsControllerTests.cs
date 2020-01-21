using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notifications.Common.Models;
using Notifications.Common.Models.Enums;
using Notifications.DataAccess;
using Notifications.DataAccess.Entities;
using Xunit;

namespace Notifications.IntegrationTests.Controllers
{
    public class NotificationsControllerTests
    {
        public NotificationsControllerTests()
        {
            var server = new TestServer(new WebHostBuilder().UseConfiguration(new ConfigurationRoot(
                new List<IConfigurationProvider>
                {
                    new MemoryConfigurationProvider(new MemoryConfigurationSource
                    {
                        InitialData = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("UseInMemoryTestingDb", "true")
                        }
                    })
                })).UseStartup<Startup>());

            client = server.CreateClient();

            dbContext = server.Host.Services.GetRequiredService<NotificationsDbContext>();

            notificationEntities = new List<NotificationEntity>
            {
                new NotificationEntity
                {
                    EventType = NotificationEventType.AppointmentCancelled,
                    UserId = Guid.NewGuid()
                },
                new NotificationEntity
                {
                    EventType = NotificationEventType.AppointmentCancelled,
                    UserId = Guid.NewGuid()
                },
                new NotificationEntity
                {
                    EventType = NotificationEventType.AppointmentCancelled,
                    UserId = currentUserId
                },
                new NotificationEntity
                {
                    EventType = NotificationEventType.AppointmentCancelled,
                    UserId = currentUserId
                }
            };
            dbContext.Notifications.AddRange(notificationEntities);

            dbContext.SaveChanges();
        }

        private readonly List<NotificationEntity> notificationEntities;
        private readonly Guid currentUserId = Guid.NewGuid();
        private readonly NotificationsDbContext dbContext;
        private readonly string notificationsUri = "/api/notifications/";
        private HttpClient client { get; }

        [Fact]
        public async Task Get_ReturnsAllNotifications()
        {
            var expectedNotifications = notificationEntities;
            var response = await client.GetAsync(notificationsUri);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<IList<NotificationModel>>(content);

            Assert.Equal(expectedNotifications.Count, responseModel.Count);
            Assert.All(expectedNotifications,
                notificationEntity => Assert.Contains(responseModel, n => notificationEntity.Id == n.Id));
        }

        [Fact]
        public async Task GetByUserId_ReturnsNotificationsFilteredForThatUser()
        {
            var expectedNotifications = notificationEntities.Where(x => x.UserId == currentUserId).ToList();
            var response = await client.GetAsync(string.Concat(notificationsUri, currentUserId));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<IList<NotificationModel>>(content);

            Assert.Equal(expectedNotifications.Count(), responseModel.Count);
            Assert.All(expectedNotifications,
                notificationEntity => Assert.Contains(responseModel, n => notificationEntity.Id == n.Id));
        }

        [Fact]
        public async Task Post_CreatesAndPersistsANewNotification()
        {
            // Arrange
            dbContext.NotificationTemplates.Add(new NotificationTemplateEntity
            {
                Id = Guid.NewGuid(),
                EventType = NotificationEventType.AppointmentCancelled,
                Body =
                    "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                Title = "Appointment Cancelled"
            });
            dbContext.SaveChanges();

            var notificationsBeforePost = dbContext.Notifications.Count();
            var eventBody = new EventModel
            {
                EventType = NotificationEventType.AppointmentCancelled,
                UserId = Guid.NewGuid(),
                Data = new EventDataModel
                {
                    AppointmentDateTime = DateTime.UtcNow,
                    Firstname = "Mr T",
                    OrganisationName = "The A Team",
                    Reason = "Too many fools pitied"
                }
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(eventBody,
                    new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()}),
                Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(notificationsUri, requestContent);

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<NotificationModel>(content);

            Assert.NotEqual(Guid.Empty, responseModel.Id);
            Assert.Contains(eventBody.Data.Reason, responseModel.Body);
            Assert.Equal(notificationsBeforePost + 1, dbContext.Notifications.Count());
        }

        [Fact]
        public async Task Post_WithUnsupportedEventType_IsNotSuccessful()
        {
            // Arrange
            var notificationsBeforePost = dbContext.Notifications.Count();
            var eventBody = new EventModel
            {
                EventType = NotificationEventType.AppointmentCancelled,
                UserId = Guid.NewGuid(),
                Data = new EventDataModel
                {
                    AppointmentDateTime = DateTime.UtcNow,
                    Firstname = "Mr T",
                    OrganisationName = "The A Team",
                    Reason = "Too many fools pitied"
                }
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(eventBody,
                    new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()}),
                Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(notificationsUri, requestContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(notificationsBeforePost, dbContext.Notifications.Count());
        }
    }
}