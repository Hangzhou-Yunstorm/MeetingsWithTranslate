using Yunstorm.Meeting.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Yunstorm.Meeting
{
    [DependsOn(
        typeof(MeetingEntityFrameworkCoreTestModule)
        )]
    public class MeetingDomainTestModule : AbpModule
    {

    }
}