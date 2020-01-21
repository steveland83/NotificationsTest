using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Common.Interfaces;
using Notifications.Common.Settings;
using Notifications.DataAccess;
using Notifications.DataAccess.Access;
using Notifications.DataAccess.Mapping;
using Notifications.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace Notifications
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Notifications API", Version = "v1"});
            });

            var notificationDbConfig = Configuration.Get<NotificationDbConfig>();
            if (notificationDbConfig.UseInMemoryTestingDb)
                services.AddDbContext<NotificationsDbContext>
                    (options => options.UseInMemoryDatabase("TestDatabase"));
            else
                services.AddDbContext<NotificationsDbContext>
                    (options => options.UseSqlServer(notificationDbConfig.NotificationsDbConnection));

            services.AddTransient<INotificationsAccess, NotificationsAccess>();
            services.AddTransient<INotificationTemplatesAccess, NotificationTemplatesAccess>();
            services.AddTransient<INotificationsService, NotificationsService>();

            services.AddAutoMapper(typeof(NotificationMappingProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notifications API V1"); });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}