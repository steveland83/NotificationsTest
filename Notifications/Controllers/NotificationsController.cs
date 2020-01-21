using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notifications.Common.Exceptions;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<NotificationModel>> Get()
        {
            return await _notificationsService.GetAllNotifications();
        }

        [HttpGet("{userId}")]
        public async Task<IReadOnlyCollection<NotificationModel>> Get([FromRoute] Guid userId)
        {
            return await _notificationsService.GetUserNotifications(userId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventModel eventModel)
        {
            try
            {
                var notification = await _notificationsService.CreateEventNotification(eventModel);
                return Ok(notification);
            }
            catch (EventTypeNotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}