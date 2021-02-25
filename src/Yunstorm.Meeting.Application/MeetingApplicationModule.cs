using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Yunstorm.Meeting
{
    [DependsOn(
        typeof(MeetingDomainModule),
        typeof(MeetingApplicationContractsModule)
        )]
    public class MeetingApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<MeetingApplicationModule>();
            });
        }
    }
}
