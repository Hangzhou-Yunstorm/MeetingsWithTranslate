using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Yunstorm.Meeting
{
    public class MeetingWebTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<MeetingWebTestModule>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}