using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Yunstorm.Meeting.EntityFrameworkCore
{
    [DependsOn(
        typeof(MeetingEntityFrameworkCoreModule)
        )]
    public class MeetingEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MeetingMigrationsDbContext>();
        }
    }
}
