using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Yunstorm.Meeting.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class MeetingConsoleApiClientModule : AbpModule
    {
        
    }
}
