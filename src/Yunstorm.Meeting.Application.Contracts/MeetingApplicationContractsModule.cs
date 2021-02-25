using Volo.Abp.Modularity;

namespace Yunstorm.Meeting
{
    [DependsOn(
        typeof(MeetingDomainSharedModule)
    )]
    public class MeetingApplicationContractsModule : AbpModule
    {

    }
}
