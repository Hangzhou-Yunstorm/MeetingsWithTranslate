using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using Yunstorm.Meeting.Meeting;

namespace Yunstorm.Meeting.EntityFrameworkCore
{
    [DependsOn(
        typeof(MeetingDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule),
        typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule)
        )]
    public class MeetingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MeetingDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories();
                options.AddRepository<MeetingRoom, EfCoreMeetingRoomRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also MeetingMigrationsDbContextFactory for EF Core tooling. */
                options.UsePostgreSql();
            });
        }
    }
}
