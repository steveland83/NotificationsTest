﻿using System;
using Microsoft.AspNetCore.Mvc;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this._notificationsService = notificationsService;
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
    }
}