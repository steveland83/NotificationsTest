using AutoMapper;

namespace Notifications.DataAccess.Mapping
{
    public class MapperConfig
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<NotificationMappingProfile>(); });

            config.AssertConfigurationIsValid();

            return config;
        }
    }
}