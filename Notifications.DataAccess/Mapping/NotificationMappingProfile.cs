using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Notifications.Common.Models;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess.Mapping
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<NotificationEntity, NotificationModel>().ReverseMap();
            CreateMap<NotificationTemplateEntity, NotificationTemplateModel>().ReverseMap();
        }
    }
}
