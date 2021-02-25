using Volo.Abp.Modularity;

namespace Yunstorm.Meeting
{
    [DependsOn(
        typeof(MeetingApplicationModule),
        typeof(MeetingDomainTestModule)
        )]
    public class MeetingApplicationTestModule : AbpModule
    {

    }
}