using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Yunstorm.Meeting.Data
{
    /* This is used if database provider does't define
     * IMeetingDbSchemaMigrator implementation.
     */
    public class NullMeetingDbSchemaMigrator : IMeetingDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}