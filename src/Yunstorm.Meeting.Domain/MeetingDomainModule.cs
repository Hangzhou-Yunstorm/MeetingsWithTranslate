using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Yunstorm.Meeting
{
    [DependsOn(
        typeof(MeetingDomainSharedModule),
        typeof(AbpBackgroundJobsDomainModule)
        )]
    public class MeetingDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = false;
            });
        }
    }
}
