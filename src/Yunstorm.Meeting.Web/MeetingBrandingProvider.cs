using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace Yunstorm.Meeting.Web
{
    [Dependency(ReplaceServices = true)]
    public class MeetingBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Meeting";
    }
}
