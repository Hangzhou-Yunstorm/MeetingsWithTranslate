using Yunstorm.Meeting.Localization;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Yunstorm.Meeting
{
    [DependsOn(
        typeof(AbpBackgroundJobsDomainSharedModule)
        )]
    public class MeetingDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MeetingDomainSharedModule>("Yunstorm.Meeting");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MeetingResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Meeting");
            });
        }
    }
}
