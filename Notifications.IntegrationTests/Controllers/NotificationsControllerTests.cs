using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

            var dbContext = server.Host.Services.GetRequiredService<NotificationsDbContext>();

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
        private HttpClient client { get; }

        [Fact]
        public async Task Get_ReturnsAllNotifications()
        {
            var expectedNotifications = notificationEntities;
            var response = await client.GetAsync("/api/notifications");

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
            var response = await client.GetAsync($"/api/notifications/{currentUserId}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<IList<NotificationModel>>(content);

            Assert.Equal(expectedNotifications.Count(), responseModel.Count);
            Assert.All(expectedNotifications,
                notificationEntity => Assert.Contains(responseModel, n => notificationEntity.Id == n.Id));
        }
    }
}